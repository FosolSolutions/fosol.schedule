using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fosol.Schedule.API.Controllers
{
    /// <summary>
    /// ErrorController class, provides endpoints to handle general HTTP request endpiont errors.
    /// </summary>
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an ErrorController object.
        /// </summary>
        public ErrorController()
        {
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// A default page to view different types of errors.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet("{statusCode?}")]
        public IActionResult Index(int statusCode)
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();
            switch (statusCode)
            {
                case ((int)HttpStatusCode.NotFound):
                    return View("404-NotFound");
                case ((int)HttpStatusCode.Unauthorized):
                    return View("401-Unauthorized");
                case ((int)HttpStatusCode.Forbidden):
                    return View("403-Forbidden");
                case ((int)HttpStatusCode.InternalServerError):
                default:
                    return View();
            }
        }
        #endregion
    }
}
