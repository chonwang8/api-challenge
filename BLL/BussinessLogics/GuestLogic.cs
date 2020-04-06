using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace BLL.BussinessLogics
{
    public class GuestLogic : IGuestLogic
    {
        #region Classes and Constructor
        protected readonly IUnitOfWork _uow;
        protected readonly IOptions<AppSetting> _options;
        protected readonly IOptions<AdminGuide> _help;

        public GuestLogic(IUnitOfWork uow, IOptions<AppSetting> options, IOptions<AdminGuide> help)
        {
            _uow = uow;
            _options = options;
            _help = help;
        }
        #endregion



        public string Login(UserLogin user)
        {
            TokenManager tokenManager = new TokenManager(_options);
            string tokenString;
            User loggedUser;

            //  Query user information from database
            try
            {
                loggedUser = _uow
                .GetRepository<User>()
                .GetAll()
                .Include(u => u.Position)
                .FirstOrDefault(u => u.Email == user.Email && u.ConfirmationCode == user.ConfirmationCode);
            }
            catch (Exception)
            {
                throw new Exception("Unable to retrieve user information");
            }

            //  if user is not found
            if (loggedUser == null)
            {
                return null;
            }

            tokenString = tokenManager.CreateAccessToken(new UserProfile
            {
                Id = loggedUser.UserId,
                Email = loggedUser.Email,
                PositionName = loggedUser.Position.Name
            }
            );

            return loggedUser.Position.Name == "admin" ? tokenString + "\n\n" + _help.Value.Message : tokenString;
        }



        public object Register(UserRegister user)
        {
            Guid positionId = new Guid();
            User newUser = new User();

            //  Check null and decline admin role
            if (user == null || user.PositionName.ToLower() == "admin")
            {
                return null;
            }

            //  Check duplicated email
            try
            {
                var checkEmail = _uow
                    .GetRepository<User>()
                    .GetAll()
                    .FirstOrDefault(u => u.Email == user.Email);
                if (checkEmail != null)
                {
                    return new ErrorModel("Email is already registered.");
                }
            }
            catch (Exception)
            {
                throw new Exception("Server Error, Please Try Again Later");
            }


            //  Check user position
            try
            {
                var checkPositionId = _uow
                    .GetRepository<Position>()
                    .GetAll()
                    .FirstOrDefault(p => p.Name == user.PositionName);
                if (checkPositionId == null)
                {
                    return new ErrorModel("Position not found.");
                }
                positionId = checkPositionId.PositionId;
            }
            catch (Exception)
            {
                throw new Exception("Server Error, Please Try Again Later");
            }

            //  Generate ConfirmationCode
            string newConfimationCode = new ConfirmationCodeManager().GenerateConfimationCode();

            newUser = new User
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                PositionId = positionId,
                UserId = Guid.NewGuid(),
                ConfirmationCode = newConfimationCode,
                DateCreate = DateTime.Now
            };

            //  Insert newUser to database
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
