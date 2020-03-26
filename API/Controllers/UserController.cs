using Amazon.S3;
using API.Attributes;
using BLL.BussinessLogics;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [UserAuthorizeFilter("junior, mid-level, senior")]
    public class UserController : BaseController
    {
        #region classes and contructor
        public UserController(IUserLogic userLogic
            , IOptions<AppSetting> options) : base(userLogic, options)
        {
        }
        #endregion



        /// <summary>
        /// Upload Cv file 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Status</returns>
        ///  /// <response code="200">Success upload file</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        #region repCode 200 400 401 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<IActionResult> UploadCV(IFormFile file)
        {
            UserClaimInfo claimInfo = new UserClaimInfo();
            UserProfile userProfile = claimInfo.GetProfile(HttpContext.User.Identity as ClaimsIdentity);
            
            if (userProfile.Email == null)
            {
                return BadRequest("User not have pemission to access to this challenge");
            }
            if (file.Length == 0)
            {
                return BadRequest("please provide valid file");
            }
            var fileName = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .TrimStart().Trim('"').ToString();
            bool status = false;

            using (var fileStream = file.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                try
                {
                    await fileStream.CopyToAsync(ms);
                    status = await _userLogic.WritingAnObjectAsync(ms, fileName, userProfile);
                }
                catch (PostgresException pgs)
                {
                    return BadRequest("PostgresException\n\n" + pgs.Message + "\n" + pgs.StackTrace);
                }
                catch (DbUpdateException dbu)
                {
                    return BadRequest("DbUpdateException\n\n" + dbu.Message + "\n" + dbu.StackTrace);
                }

            }
            return status ? Ok("success")
                          : StatusCode((int)HttpStatusCode.InternalServerError, $"error uploading {fileName}");
        }



        /// <summary>
        /// Download CV file
        /// </summary>
        /// <returns>File</returns>
        /// <response code="200">Success upload file</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="404">File Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        #region repCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetUserCvUrl()
        {
            UserClaimInfo claimInfo = new UserClaimInfo();
            UserProfile userProfile = claimInfo.GetProfile(HttpContext.User.Identity as ClaimsIdentity);
            #region Check User
            
            if (userProfile.Id == null)
            {
                return BadRequest("Missing User Id");
            }
            if (userProfile.Email == null)
            {
                return BadRequest("Missing Email");
            }
            #endregion
            string response = "";
            try
            {
                response = _userLogic.ReadFileUrlAsync(userProfile.Id);
                if (response == null)
                    return NotFound("User does not have Cv yet");
            }
            catch (AmazonS3Exception)
            {
                return BadRequest("Unable to retrieve file");
            }
            catch (NullReferenceException)
            {
                return NotFound("Cv not found");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }

            return Ok(response);
        }
    }



    [UserAuthorizeFilter("junior, mid-level, senior")]
    public class ChallengesController : BaseController
    {
        #region classes and contructor
        public ChallengesController(IUserLogic userLogic, IOptions<AppSetting> options) 
            : base(userLogic, options)
        {
        }
        #endregion


        /// <summary>
        /// View List of Challeges of User's position
        /// </summary>
        /// <returns>List of challenges</returns>
        /// <response code="200">List of challenge</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="403">Forbidden from resource</response>
        /// <response code="404">Empty challenge list</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        #region RepCode 200 400 401 404 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult ViewChallengeList()
        {
            UserClaimInfo claimInfo = new UserClaimInfo();
            UserProfile userProfile = claimInfo.GetProfile(HttpContext.User.Identity as ClaimsIdentity);
            if (userProfile.Email == null || userProfile.PositionName == null)
            {
                return BadRequest("User missing some vital infomation to use this feature");
            }

            var challenges = _userLogic.ViewChallengesList(userProfile);
            if (challenges == null)
            {
                return NotFound("There are no Challenge for this position");
            }
            return Ok(challenges);
        }



        /// <summary>
        /// View Challenge's context
        /// </summary>
        /// <returns>Challenge's context</returns>
        /// <response code="200">Challenge Description</response>
        /// <response code="400">Not have enough infomation</response>
        /// <response code="401">Unauthorize</response>
        /// <response code="403">User's position different from challenge's position</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        #region RepCode 200 400 401 403 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult GetChallengeDetail(Guid id)
        {
            UserClaimInfo claimInfo = new UserClaimInfo();
            UserProfile userProfile = claimInfo.GetProfile(HttpContext.User.Identity as ClaimsIdentity);
            if (userProfile.PositionName == null)
            {
                return BadRequest("User not have pemission to access to this challenge");
            }

            var chal = _userLogic.ViewChallengeContent(id);

            if (!userProfile.PositionName.Equals(chal.PositionName))
            {
                return Forbid("Permission denied");
            }
            ChallengeInfoView challenge = new ChallengeInfoView
            {
                ChallengeName = chal.ChallengeName,
                ChallengeDescription = chal.ChallengeDescription
            };
            return Ok(challenge);
        }

    }
}