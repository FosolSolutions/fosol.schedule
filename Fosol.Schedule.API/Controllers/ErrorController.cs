using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Controllers
{

    [Route("[controller]")]
    public class ErrorController : Controller
    {
        #region Variables
        #endregion

        #region Constructors
        public ErrorController()
        {
        }
        #endregion

        #region Endpoints
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
