using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace API.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        #region Constructor that takes GuestLogic, HelpPage
        IOptions<LoginGuide> _loginGuide;
        public LoginController(IGuestLogic guestLogic,
            IOptions<HelpPage> helpPage,
            IOptions<LoginGuide> login) : base(guestLogic, helpPage)
        {
            _loginGuide = login;
        }
        #endregion



        [HttpGet]
        public IActionResult Help()
        {
            string login = _loginGuide.Value.Message;
            return Ok(login);
        }



        /// <summary>
        /// Login to get Access Token
        /// </summary>
        /// <remarks>
        /// 
        /// Sample Request:
        /// 
        /// 
        ///     {
        ///         "email" : "name@name.com"
        ///         "confirmationCode" : "string"
        ///     }
        ///     
        /// </remarks>
        /// <returns>Access token</returns>
        /// <response code="200">Logged in</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="404">User Not Exist</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        #region RepCode 200 400 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult Login(string email, string confirmationCode)
        {
            UserLogin user = new UserLogin
            {
                Email = email,
                ConfirmationCode = confirmationCode,
            };
            string response;

            //  Check For Bad Inputs
            if (user.Email == null || user.ConfirmationCode == null)
            {
                return BadRequest("Error : Can not get appropriate email or confirmation code");
            }
            if (user.Email.Length <= 0 || user.ConfirmationCode.Length <= 0)
            {
                return BadRequest("Email and ConfimationCode can not be empty");
            }

            //  Login. Response token string if succeed. Null if not found
            //  Returns exception when database is down.
            try
            {
                response = _logic.Login(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            if (response == null)
                return Unauthorized("Login Failed. Please check Email or Confimation code");
            else
                return Ok(response);
        }
    }

    [AllowAnonymous]
    public class RegisterController : BaseController
    {
        #region Constructor that takes GuestLogic, HelpPage
        IOptions<RegisterGuide> _registerGuide;
        public RegisterController(IGuestLogic logic,
            IOptions<HelpPage> helpPage,
            IOptions<RegisterGuide> register) : base(logic, helpPage)
        {
            _registerGuide = register;
        }
        #endregion



        [HttpGet]
        public IActionResult Help()
        {
            string loginFormat = _registerGuide.Value.Message;
            return Ok(loginFormat);
        }



        /// <summary>
        /// Register with Email - Phone - FullName - PositionName
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///     {
        ///         "Email" : "name@name.com",
        ///         "Phone" : "123456"
        ///         "FullName" : "John Doe"
        ///         "PositionName" : "Junior"
        ///     }
        /// </remarks>
        /// <returns>ConfirmationCode</returns>
        /// <response code="200">Successfully registered. All info valid.</response>
        /// <response code="400">Invalid Input</response>
        /// <response code="404">Server Denied Access</response>
        /// <response code="500">Server Is Down</response>
        [HttpPost]
        [AllowAnonymous]
        #region RepCode 200 400 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult Register(UserRegister user)
        {
            //  Input : UserRegister includes :
            //  Email - Phone - FullName - PositionName
            UserLogin userLogin = new UserLogin();

            //  check input from client
            if (user == null || user.PositionName.ToLower() == "admin")
            {
                return BadRequest("Invalid Input");
            }
            if (user.Email.Length <= 8)
            {
                return BadRequest("Email and ConfimationCode must be 8 or more characters");
            }

            //  Register function
            try
            {
                var userRegisterResponse = _logic.Register(user);
                if (userRegisterResponse is ErrorModel)
                    return BadRequest(userRegisterResponse);
                else
                    return Ok(userRegisterResponse);
            }
            //  GuestLogic received UserRegister = null
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}