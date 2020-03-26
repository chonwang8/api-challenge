using Amazon.S3;
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


        /// <summary>
        /// Get list of candidates - Ordered by Candidate's Account Created Date
        /// </summary>
        /// <returns>Create date</returns>
        /// <response code="200">return list</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">Not found any user</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        #region RepCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetCandidates()
        {
            var apm = new AdminPageModel();
            try
            {
                List<PageModel> pageModels = _adminLogic.GetPageModelList();
                if (pageModels.Count == 0)
                    return NotFound("There are no Candidates");
                apm.UserList = pageModels;
                return Ok(apm);
            }
            catch (Exception)
            {
                return BadRequest("System Error: Please check inputs and connection");
            }
        }



        /// <summary>
        /// Get list of candidates - Ordered by Input Value (string INDEX(nullable)). 
        /// Available INDEXs : DateCreate, Email, FullName, Position
        /// </summary>
        /// Sample Request:
        ///     {
        ///         "pageItems" : "1",
        ///         "page" : "1",
        ///         "INDEX" : "Email"
        ///     }
        /// <response code="200">return list</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">Not found any user</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("sort")]
        #region RepCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetCandidatesSort(string pageItems, string page, string INDEX)
        {
            int PageItems, Page;
            var apm = new AdminPageModel();
            List<PageModel> pageModels = new List<PageModel>();
            #region Set default paging values if null or empty input
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
            #endregion

            try
            {
                pageModels = _adminLogic.GetPageModelSortList(PageItems, Page, INDEX); if (pageModels.Count == 0)
                {
                    return NotFound("There are no user");
                }
                apm.UserList = pageModels;

                return Ok(apm);
            }
            catch (NullReferenceException)
            {
                return BadRequest("Error : Input Reference not found");

            }
            catch (Exception)
            {
                return BadRequest("System Error: Please check inputs and connection");
            }
        }



        /// <summary>
        /// Get list of candidates - Ordered by Input Value (string INDEX(nullable)). Available INDEXs : DateCreate, Email, FullName, Position
        /// - Filtered by Input Position
        /// </summary>
        /// Sample Request:
        ///     {
        ///         "pageItems" : "1",
        ///         "page" : "1",
        ///         "INDEX" : "Email",
        ///         "Position" : "junior"
        ///     }
        /// <returns>Create Position</returns>
        /// <response code="200">return list</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">Not found any user</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("role")]
        #region RepCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetCandidatesSortFilter(string pageItems, string page, string INDEX, string POSITION)
        {
            int PageItems, Page;
            var apm = new AdminPageModel();

            #region Set default paging values if null or empty input
            //  pageItems
            try
            {
                PageItems = (pageItems == null || pageItems.Length <= 0) ? 40 : int.Parse(pageItems);
            }
            catch (Exception)
            {
                return BadRequest("Number of Items per page must be in digits only");
            }
            //  page
            try
            {
                Page = (page == null || page.Length <= 0) ? 1 : int.Parse(page);
            }
            catch (Exception)
            {
                return BadRequest("Page Number must be in digits only");
            }
            #endregion

            #region Check Candidate's position input
            //  Check Valid Input
            if (POSITION == null || POSITION.Length <= 0)
            {
                return BadRequest("Must input proper candidate's position");
            }
            //  Check Valid Position Input
            if (POSITION != "junior" && POSITION != "mid-level" && POSITION != "senior")
            {
                return BadRequest("Invalid Candidate's Position - only 'junior', 'mid-level', 'senior' allowed");
            }
            #endregion

            try
            {
                List<PageModel> pageModels = _adminLogic.GetPageModelFilterList(PageItems, Page, INDEX, POSITION);
                if (pageModels.Count == 0)
                {
                    return NotFound("There are no candidates in this position");
                }
                apm.UserList = pageModels;
                return Ok(apm);
            }
            catch (ArgumentException)
            {
                return BadRequest("Input Error");
            }
            catch (NullReferenceException)
            {
                return BadRequest("Error : Input Reference Not Found");
            }
            catch (Exception)
            {
                return BadRequest("System Error: Please check inputs and connection");
            }
        }



        /// <summary>
        /// Search list of user with name similar to input
        /// </summary>
        /// Sample Request:
        ///     {
        ///         "pageItems" : "1",
        ///         "page" : "1",
        ///         "NAME" : "Wang"
        ///     }
        /// <response code="200">return list</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">Not found any user</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("name")]
        #region RepCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetCandidatesSearch(string pageItems, string page, string NAME)
        {
            int PageItems, Page;

            #region Set default paging values if null or empty input
            //  pageItems
            try
            {
                PageItems = (pageItems == null || pageItems.Length <= 0) ? 99 : int.Parse(pageItems);
            }
            catch (Exception)
            {
                return BadRequest("Number of Items per page must be in digits only");
            }
            //  page
            try
            {
                Page = (page == null || page.Length <= 0) ? 1 : int.Parse(page);
            }
            catch (Exception)
            {
                return BadRequest("Page Number must be in digits only");
            }
            #endregion

            var apm = new AdminPageModel();

            try
            {
                List<PageModel> pageModels = _adminLogic.GetPageModelSearchList(PageItems, Page, NAME);
                if (pageModels.Count == 0)
                {
                    return NotFound("There no user with name : " + NAME);
                }

                apm.UserList = pageModels;
                return Ok(apm);
            }
            catch (Exception)
            {
                return BadRequest("System Error: Please check inputs and connection");
            }
        }



        /// <summary>
        /// Download Candidate's CV file
        /// </summary>
        /// Sample Request:
        ///     {
        ///         "Id" : "0000-0000000-00000-00000"
        ///     }
        /// <returns>File</returns>
        /// <response code="200">Success upload file</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">File Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("cv")]
        #region repCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetCandidateCv(Guid Id)
        {
            string response = "";

            // Check UserID Input
            if (Id == null)
            {
                return BadRequest("Missing User Id");
            }

            try
            {
                response = _userLogic.ReadFileUrlAsync(Id);
            }
            catch (AmazonS3Exception)
            {
                return BadRequest("Unable to retrieve file");
            }
            catch (NullReferenceException)
            {
                return NotFound("User CV Not Found");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }

            return Ok(response);
        }
    }
}