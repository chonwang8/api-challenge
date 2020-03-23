using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace BLL.BussinessLogics
{
    public class GuestLogic : IGuestLogic
    {
        #region OnCall
        protected readonly IUnitOfWork _uow;
        protected readonly IOptions<AppSetting> _options;

        public GuestLogic(IUnitOfWork uow, IOptions<AppSetting> options)
        {
            _uow = uow;
            _options = options;
        }
        #endregion


        public string Login(UserLogin user)
        {
            User loggedUser = _uow
                .GetRepository<User>()
                .GetAll()
                .SingleOrDefault(u => u.Email == user.Email && u.ConfirmationCode == user.ConfirmationCode);
            if (loggedUser == null)
            {
                throw new Exception("Incorrect Email or Password");
            }

            string positionName = _uow
                .GetRepository<Position>()
                .GetAll()
                .SingleOrDefault(p => p.PositionId == loggedUser.PositionId).Name;
            if (positionName == null)
            {
                throw new ArgumentException("Invalid Position Applied");
            }

            UserProfile userProfile = new UserProfile
            {
                Id = loggedUser.UserId,
                Email = loggedUser.Email,
                PositionName = positionName
            };

            TokenManager tokenManager = new TokenManager(_options);
            string tokenString = tokenManager.CreateAccessToken(userProfile);
            return tokenString;
        }

        public UserLogin Register(UserRegister user)
        {
            Guid positionId = new Guid();
            if (user == null || user.PositionName.ToLower() == "admin")
            {
                throw new ArgumentNullException("Invalid Acccount Input");
            }

            try
            {
                positionId = _uow
                .GetRepository<Position>()
                .GetAll()
                .SingleOrDefault(p => p.Name == user.PositionName).PositionId;
            }
            catch (InvalidOperationException ioe)
            {
                throw ioe;
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            catch (Exception e)
            {
                throw e;
            }


            string newCofimationCode = new ConfirmationCodeManager().GenerateConfimationCode();

            //  Use domain class for now, will implement mapper later
            User newUser = new User
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                PositionId = positionId,
                UserId = Guid.NewGuid(),
                ConfirmationCode = newCofimationCode,
                DateCreate = DateTime.Now
            };

            _uow.GetRepository<User>().Insert(newUser);
            _uow.Commit();
            return new UserLogin
            {
                Email = newUser.Email,
                ConfirmationCode = newUser.ConfirmationCode
            };
        }
    }
}
