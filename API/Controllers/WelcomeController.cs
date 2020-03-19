using BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{

    [Route("start")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        protected readonly IOptions<IndexPage> _indexPage;

        public WelcomeController(IOptions<IndexPage> indexPage)
        {
            _indexPage = indexPage;
        }


        /// <summary>
        /// View Start page
        /// </summary>
        /// <returns>Start page</returns>
        /// <response code="200">Start page</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        #region RepCode 200 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult Guest()
        {
            var index = _indexPage.Value;
            return Ok(index.Message);
        }
    }


    [Route("help")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        protected readonly IOptions<HelpPage> _helpPage;

        public HelpController(IOptions<HelpPage> helpPage)
        {
            _helpPage = helpPage;
        }


        /// <summary>
        /// View Help page
        /// </summary>
        /// <returns>Help page</returns>
        /// <response code="200">Help page</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        #region RepCode 200 500
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public IActionResult Help()
        {
            var helplist = _helpPage.Value;
            string contentList = string.Join("\n\n", helplist.ContentList.ToArray());

            return Ok(helplist.Header + "\n\n"
                + helplist.Guildline + "\n\n"
                + contentList.ToString());
        }


        [HttpGet("login")]
        public IActionResult LoginHelp()
        {
            string registerFormat = "POST /Login" +
                "\n{" +
                "\n    \"email\": \"string\"" +
                "\n    \"confirmationCode\": \"string\"" +
                "\n}";
            return Ok(registerFormat);
        }



        [HttpGet("register")]
        public IActionResult RegisterHelp()
        {
            string loginFormat = "Positions we are hiring : Junior, Mid-level, Senior" +
                "\n\nPOST /Register" +
                "\n{" +
                "\n    \"phone\": \"string\"" +
                "\n    \"fullName\": \"string\"" +
                "\n    \"positionName\": \"string\"" +
                "\n    \"email\": \"string\"" +
                "\n}";
            return Ok(loginFormat);
        }
    }




}