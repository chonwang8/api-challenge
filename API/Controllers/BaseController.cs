﻿using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region objects and constructors
        protected const string DATE = "date";
        protected const string EMAIL = "email";
        protected const string POSITION = "position";

        protected readonly IGuestLogic _logic;
        protected readonly IAdminLogic _adminLogic;
        protected readonly IUserLogic _userLogic;
        protected readonly IOptions<HelpPage> _helpPage;
        protected readonly IOptions<IndexPage> _indexPage;
        protected readonly IOptions<AppSetting> _options;
        protected readonly IHttpContextAccessor _httpContext;

        public BaseController(IGuestLogic logic)
        {
            _logic = logic;
        }

        public BaseController(IOptions<HelpPage> helpPage)
        {
            _helpPage = helpPage;
        }

        public BaseController(IOptions<IndexPage> indexPage)
        {
            _indexPage = indexPage;
        }

        public BaseController(IGuestLogic guestLogic, IOptions<HelpPage> helpPage)
        {
            _logic = guestLogic;
            _helpPage = helpPage;
        }


        //  For UserController
        public BaseController(IUserLogic userLogic, IOptions<AppSetting> options)
        {
            _userLogic = userLogic;
            _options = options;
        }


        //  For AdminController
        public BaseController(IAdminLogic adminLogic, IUserLogic userLogic, IHttpContextAccessor httpContext)
        {
            _adminLogic = adminLogic;
            _userLogic = userLogic;
            _httpContext = httpContext;
        }


        #endregion

    }
}