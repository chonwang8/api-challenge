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
            List<PageModel> pageModels = _adminLogic.GetPageModelList();

            if (pageModels == null)
            {
                return BadRequest("System Error: Please check inputs or connection");
            }
            if (pageModels.Count == 0)
            {
                return NotFound("There are no Candidates");
            }
            apm.UserList = pageModels;

            return Ok(apm);

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
        public IActionResult GetCandidatesSort(int pageItems, int page, string INDEX)
        {
            var apm = new AdminPageModel();
            List<PageModel> pageModels = new List<PageModel>();

            #region Check Paging Input
            if (pageItems <= 0)
            {
                return BadRequest("Paging error : Items per page must be positive integer");
            }
            if (page <= 0)
            {
                return BadRequest("Paging error : Page number must be positive integer");
            }
            #endregion

            pageModels = _adminLogic.GetPageModelSortList(pageItems, page, INDEX);
            if (pageModels == null)
            {
                return BadRequest("Error : Input Reference not found");
            }
            if (pageModels.Count == 0)
            {
                return NotFound("There are no user");
            }
            apm.UserList = pageModels;

            return Ok(apm);

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
        public IActionResult GetCandidatesSortFilter(int pageItems, int page, string INDEX, string POSITION)
        {
            var apm = new AdminPageModel();

            #region Check Paging Input
            if (pageItems <= 0)
            {
                return BadRequest("Paging error : Items per page must be positive integer");
            }
            if (page <= 0)
            {
                return BadRequest("Paging error : Page number must be positive integer");
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

            List<PageModel> pageModels = _adminLogic.GetPageModelFilterList(pageItems, page, INDEX, POSITION);
            if (pageModels == null)
            {
                return BadRequest("Error : Input Reference Not Found");
            }
            if (pageModels.Count == 0)
            {
                return NotFound("There are no candidates in this position");
            }
            apm.UserList = pageModels;
            return Ok(apm);

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
        public IActionResult GetCandidatesSearch(int pageItems, int page, string NAME)
        {

            #region Check Paging Input
            if (pageItems <= 0)
            {
                return BadRequest("Paging error : Items per page must be positive integer");
            }
            if (page <= 0)
            {
                return BadRequest("Paging error : Page number must be positive integer");
            }
            #endregion

            var apm = new AdminPageModel();

            List<PageModel> pageModels = _adminLogic.GetPageModelSearchList(pageItems, page, NAME);

            if (pageModels == null)
            {
                return BadRequest("Error : Input Reference Not Found");
            }
            if (pageModels.Count == 0)
            {
                return NotFound("There no user with name : " + NAME);
            }

            apm.UserList = pageModels;
            return Ok(apm);

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