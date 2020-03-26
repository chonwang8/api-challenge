using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.BussinessLogics
{
    public class UserLogic : IUserLogic
    {
        #region objects and constructors
        private readonly IUnitOfWork _uow;
        private AppSetting appSetting;
        public UserLogic(IUnitOfWork uow, IOptions<AppSetting> options)
        {
            _uow = uow;
            appSetting = options.Value;
        }
        #endregion


        public ChallengeContent ViewChallengeContent(Guid ChallengeId)
        {
            var challenge = _uow.GetRepository<Challenge>().GetAll().FirstOrDefault(c => c.ChallengeId == ChallengeId);
            var posName = _uow.GetRepository<Position>().GetAll().SingleOrDefault(p => p.PositionId == challenge.PositionId).Name;
            if (challenge == null)
            {
                throw new ArgumentNullException();
            }
            if (posName == null)
            {
                throw new ArgumentNullException();
            }
            ChallengeContent challengeContent = new ChallengeContent
            {
                ChallengeName = challenge.Name,
                ChallengeDescription = challenge.Content,
                PositionName = posName
            };
            return challengeContent;
        }



        public List<ChallengeProfile> ViewChallengesList(UserProfile userProfile)
        {
            string PosName = userProfile.PositionName;
            Guid posID = _uow.GetRepository<Position>().GetAll().SingleOrDefault(p => p.Name == PosName).PositionId;
            if (posID == null)
            {
                throw new ArgumentNullException();
            }
            List<Challenge> ChallengesList = _uow
                .GetRepository<Challenge>()
                .GetAll()
                .Where(c => c.PositionId == posID).ToList();
            List<ChallengeProfile> ChallengeProfilesList = ChallengesList.Select(c => new ChallengeProfile
            {
                ChallengeId = c.ChallengeId,
                ChallengeName = c.Name
            }).ToList();

            return ChallengeProfilesList;
        }


        public async Task<bool> WritingAnObjectAsync(Stream fileStream, string fileName, UserProfile userProfile)
        {
            IAmazonS3 client = new AmazonS3Client(appSetting.AWSAccessKey, appSetting.AWSSecretKey, RegionEndpoint.APSoutheast1);
            try
            {
                var fileTransferUtility = new TransferUtility(client);

                CvModel cvModel = new CvModel
                {
                    Email = userProfile.Email,
                    FileName = fileName,
                    KeyName = Guid.NewGuid(),
                    UploadDate = DateTime.Now,
                };

                var fileUploadRequest = new TransferUtilityUploadRequest()
                {
                    BucketName = appSetting.BucketName,
                    Key = userProfile.Email + "/" + fileName,
                    InputStream = fileStream,
                };
                fileUploadRequest.Metadata.Add("email", cvModel.Email);
                fileUploadRequest.Metadata.Add("upload-time", cvModel.UploadDate.ToString());
                if (userProfile.Id == null)
                {
                    return false;
                }
                await fileTransferUtility.UploadAsync(fileUploadRequest);
                #region CvTableInsert
                try
                {
                    var user = _uow.GetRepository<User>().GetAll().FirstOrDefault(u => u.UserId == userProfile.Id);
                    var existCv = _uow.GetRepository<Cv>().GetAll().FirstOrDefault(c => c.UserId == userProfile.Id);
                    var cv = new Cv
                    {
                        UserId = userProfile.Id,
                        FileName = cvModel.FileName,
                        UploadDate = cvModel.UploadDate,
                    };
                    if (existCv == null)
                    {
                        _uow.GetRepository<Cv>().Insert(cv);
                    }
                    else
                    {
                        _uow.GetRepository<Cv>().Update(cv);
                        _uow.GetRepository<User>().Update(user);
                    }
                    _uow.Commit();

                }
                catch (PostgresException pgs)
                {
                    throw pgs;
                }
                #endregion
                return true;


            }
            catch (AmazonS3Exception)
            {
                return false;
            }
            catch (PostgresException pgs)
            {
                throw pgs;
            }
        }



        public string ReadFileUrlAsync(Guid Id)
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(appSetting.AWSAccessKey, appSetting.AWSSecretKey, RegionEndpoint.APSoutheast1);
                string fileName;
                try
                {
                    fileName = _uow.GetRepository<Cv>().GetAll().FirstOrDefault(c => c.UserId == Id).FileName;
                }
                catch (Exception e)
                {
                    throw e;
                }
                
                if (fileName == null)
                {
                    return null;
                }
                var Email = _uow.GetRepository<User>().GetAll().FirstOrDefault(u => u.UserId == Id).Email;
                
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = appSetting.BucketName,
                    Key = Email + "/" + fileName,
                    Expires = DateTime.Now.AddDays(10),
                    Protocol = Protocol.HTTPS
                };

                string url = client.GetPreSignedURL(request);

                return url;
            }
            catch (AmazonS3Exception s3)
            {
                throw s3;
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
};
