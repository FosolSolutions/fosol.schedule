using Fosol.Schedule.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Controllers
{
    /// <summary>
    /// ApiController class, provides API endpoints that describe all other endpoints in this application.
    /// </summary>
    [Produces("application/json")]
    [Area("api")]
    [Route("[area]")]
    public class ApiController : Controller
    {
        #region Methods
        /// <summary>
        /// Returns an array of all the API endpoints in this application with their documentation.
        /// </summary>
        /// <returns>An array of Endpoint.</returns>
        [HttpGet("endpoints")]
        public IActionResult Endpoints()
        {
            return Ok(ApiHelper.GetEndpoints());
        }
        #endregion
    }
}