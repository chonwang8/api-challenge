using API.Attributes;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("ad")]
    [UserAuthorizeFilter("admin")]
    public class AdminController : BaseController
    {
        #region Constructor that takes AdminLogic, UserLogic, IHttpContextAccessor
        public AdminController(IAdminLogic adminLogic,
            IUserLogic userLogic,
            IHttpContextAccessor httpContext) : base(adminLogic, userLogic, httpContext)
        {
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

            if (apm.UserList == null)
            {
                return BadRequest();
            }
            if (apm.UserList.Count <= 0)
            {
                return BadRequest("There are no Candidates");
            }

            return Ok(apm);
        }

        [HttpGet("sort")]
        public IActionResult GetUserListOrderByIndex(string pageItems, string page, string INDEX)
        {
            int PageItems, Page;

            try
            {
                PageItems = (pageItems == null || pageItems.Length <= 0) ? 40 : int.Parse(pageItems);
            }
            catch (Exception)
            {
                return BadRequest("Number of Items per page must be in digits only");
            }

            try
            {
                Page = (page == null || page.Length <= 0) ? 1 : int.Parse(page);
            }
            catch (Exception)
            {
                return BadRequest("Page Number must be in digits only");
            }

            if (INDEX == null || INDEX.Length <= 0)
            {
                INDEX = "date";
            }

            var apm = new AdminPageModel();

            List<PageModel> pageModels = _adminLogic.GetPageModelSortList(PageItems, Page, INDEX);

            if (pageModels.Count == 0)
            {
                return NotFound("There are no user");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }

        [HttpGet("filter")]
        public IActionResult GetUserListFilterByPosition(string pageItems, string page, string INDEX, string position)
        {
            int PageItems, Page;

            try
            {
                PageItems = (pageItems == null || pageItems.Length <= 0) ? 40 : int.Parse(pageItems);
            }
            catch (Exception)
            {
                return BadRequest("Number of Items per page must be in digits only");
            }

            try
            {
                Page = (page == null || page.Length <= 0) ? 1 : int.Parse(page);
            }
            catch (Exception)
            {
                return BadRequest("Page Number must be in digits only");
            }

            if (INDEX == null || INDEX.Length <= 0)
            {
                INDEX = "date";
            }

            if (position == null || position.Length <= 0)
            {
                return BadRequest("Must input proper position");
            }

            var apm = new AdminPageModel();

            List<PageModel> pageModels = _adminLogic.GetPageModelFilterList(PageItems, Page, INDEX, position);

            if (pageModels.Count == 0)
            {
                return NotFound("There are no user");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }

        [HttpGet("search")]
        public IActionResult GetUserListSearchByName(string pageItems, string page, string search)
        {
            int PageItems, Page;

            try
            {
                PageItems = (pageItems == null || pageItems.Length <= 0) ? 99 : int.Parse(pageItems);
            }
            catch (Exception)
            {
                return BadRequest("Number of Items per page must be in digits only");
            }

            try
            {
                Page = (page == null || page.Length <= 0) ? 1 : int.Parse(page);
            }
            catch (Exception)
            {
                return BadRequest("Page Number must be in digits only");
            }

            var apm = new AdminPageModel();

            List<PageModel> pageModels = _adminLogic
                .GetPageModelSearchList(PageItems, Page, search);

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
            // Check UserID Input
            if (Id == null)
            {
                return BadRequest("Missing User Id");
            }
            string response = "";
            try
            {
                response = _userLogic.ReadFileUrlAsync(Id);
            }
            catch (NullReferenceException nre)
            {
                return NotFound(nre);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(response);
        }
    }
}