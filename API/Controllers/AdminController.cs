using API.Attributes;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
namespace API.Controllers
{
    [Route("admin")]
    [ApiController]
    [Authorize]
    [MyAuthorize("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAdminLogic _adminLogic;
        public AdminController(IHttpContextAccessor httpContext, IAdminLogic adminLogic)
        {
            _httpContext = httpContext;
            _adminLogic = adminLogic;
        }

        [HttpGet("date")]
        public IActionResult GetUserListOrderByCreateDate(int pageItems, int page)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic.GetPageModelList()
                                                    .OrderBy(p => p.DateCreate)
                                                    .Skip(paging.SkipItem(page, pageItems))
                                                    .Take(pageItems)
                                                    .ToList();
            if (pageModels.Count == 0)
            {
                return NotFound("There no user");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }

        [HttpGet("email")]
        public IActionResult GetUserListOrderByEmail(int pageItems, int page)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic.GetPageModelList()
                                                    .OrderBy(p => p.Email)
                                                    .Skip(paging.SkipItem(page, pageItems))
                                                    .Take(pageItems)
                                                    .ToList();
            if (pageModels.Count == 0)
            {
                return NotFound("There no user");
            }
            apm.UserList = pageModels;

            return Ok(apm);
        }

        [HttpGet("position")]
        public IActionResult GetUserListOrderByRole(int pageItems, int page)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic.GetPageModelList()
                                                    .OrderBy(p => p.Position)
                                                    .Skip(paging.SkipItem(page, pageItems))
                                                    .Take(pageItems)
                                                    .ToList();
            if (pageModels.Count == 0)
            {
                return NotFound("There no user");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }

        [HttpGet]
        public IActionResult GetSearchListOrderByName(int pageItems, int page, string search)
        {
            if (pageItems < 1 || page < 1)
            {
                return BadRequest("Number of item on page can below one");
            }
            var apm = new AdminPageModel();
            var paging = new Paging();
            List<PageModel> pageModels = _adminLogic.GetSearchPageModelList(search)
                                                    .OrderBy(p => p.FullName)
                                                    .Skip(paging.SkipItem(page, pageItems))
                                                    .Take(pageItems)
                                                    .ToList();
            if (pageModels.Count == 0)
            {
                return NotFound("There no user with " + search + " Name");
            }
            apm.UserList = pageModels;
            return Ok(apm);
        }
    }
}