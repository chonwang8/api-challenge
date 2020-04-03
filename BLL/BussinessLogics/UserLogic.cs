using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAmazonS3 S3Client;
        private readonly AppSetting _appSetting;

        public UserLogic(IUnitOfWork uow, IAmazonS3 s3Client, IOptions<AppSetting> options)
        {
            _uow = uow;
            S3Client = s3Client;
            _appSetting = options.Value;
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
            if (userProfile == null)
                return false;
            if (userProfile.Id == null)
                return false;

            var fileTransferUtility = new TransferUtility(S3Client);

            var fileUploadRequest = new TransferUtilityUploadRequest()
            {
                BucketName = _appSetting.BucketName,
                Key = userProfile.Email + "/" + fileName,
                InputStream = fileStream
            };
            fileUploadRequest.Metadata.Add("email", userProfile.Email);
            fileUploadRequest.Metadata.Add("upload-time", DateTime.Now.ToString());


                await fileTransferUtility.UploadAsync(fileUploadRequest);
                #region CvTableInsert
                var user = _uow.GetRepository<User>().GetAll().FirstOrDefault(u => u.UserId == userProfile.Id);
                var existCv = _uow.GetRepository<Cv>().GetAll().FirstOrDefault(c => c.UserId == userProfile.Id);
                if (user == null)
                {
                    return false;
                }

                var cv = new Cv
                {
                    UserId = userProfile.Id,
                    FileName = fileName,
                    UploadDate = DateTime.Now,
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
                #endregion
                return true;
        }



        public string ReadFileUrlAsync(Guid Id)
        {
            string fileName;
            string email;
            try
            {
                var cv = _uow.GetRepository<Cv>().GetAll().FirstOrDefault(c => c.UserId == Id);
                if (cv == null)
                    return null;
                else
                    fileName = cv.FileName;

                var user = _uow.GetRepository<User>().GetAll().FirstOrDefault(u => u.UserId == Id);
                if (user == null)
                    return null;
                else
                    email = user.Email;

                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = _appSetting.BucketName,
                    Key = email + "/" + fileName,
                    Expires = DateTime.Now.AddDays(10),
                    Protocol = Protocol.HTTPS
                };

                string url = S3Client.GetPreSignedURL(request);

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
