using API.Attributes;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
namespace API.Controllers
{
    [MyAuthorize("admin")]
    public class AdminController : BaseController
    {
        #region classes and constructor
        private const string DATE = "date";
        private const string EMAIL = "email";
        private const string POSITION = "position";
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAdminLogic _adminLogic;
        private readonly IUserLogic _userLogic;
        public AdminController(IHttpContextAccessor httpContext, IAdminLogic adminLogic, IUserLogic userLogic)
        {
            _httpContext = httpContext;
            _adminLogic = adminLogic;
            _userLogic = userLogic;
        }
        #endregion

        [HttpGet]
        public IActionResult GetUserList()
        {
            var apm = new AdminPageModel();
            List<PageModel> pageModels = _adminLogic.GetPageModelList();
            if (pageModels.Count == 0)
            {
                return NotFound("There are no Candidates");
            }
            apm.UserList = pageModels;

            if(apm.UserList == null)
            {
                return BadRequest();
            }
            if(apm.UserList.Count <= 0)
            {
                return BadRequest("There are no Candidates");
            }

            return Ok(apm);
        }

        [HttpGet("sort")]
        public IActionResult GetUserListOrderByIndex(int pageItems, int page, string INDEX)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            if (INDEX == null || INDEX.Length <= 0)
            {
                return BadRequest("Please input sort Criteria");
            }

            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic.GetPageModelSortList(pageItems, page, INDEX);
                                                    
            if (pageModels.Count == 0)
            {
                return NotFound("There are no user");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }


        [HttpGet("search")]
        public IActionResult GetSearchListOrderByName(int pageItems, int page, string search)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic
                .GetSearchPageModelList(pageItems, page, search);

            if (pageModels.Count == 0)
            {
                return NotFound("There no user with " + search + " Name");
            }

            if (pageModels == null)
            {
                return BadRequest("sum ting wong");
            }

            apm.UserList = pageModels;
            return Ok(apm);
        }

        [HttpGet("cv")]
        public IActionResult GetUserCv(Guid Id)
        {
            #region Check User
            if (Id == null)
            {
                return BadRequest("Missing User Id");
            }
            string response = "";
            #endregion
            try
            {

                response = _userLogic.ReadFileUrlAsync(Id);
            }
            catch(NullReferenceException nre)
            {
                return NotFound(nre);
            }

            return Ok(response);
        }
    }
}