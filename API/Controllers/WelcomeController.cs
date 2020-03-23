using BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("start")]
    public class WelcomeController : BaseController
    {
        #region Constructor that takes IndexPage
        public WelcomeController(IOptions<IndexPage> indexPage) : base(indexPage)
        {
        }
        #endregion

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
        public IActionResult Start()
        {
            var index = _indexPage.Value;
            return Ok(index.Message);
        }
    }

    [Route("help")]
    public class HelpController : BaseController
    {
        #region Constructor that takes HelpPage
        public HelpController(IOptions<HelpPage> helpPage) : base (helpPage)
        {
        }
        #endregion

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

    }
}