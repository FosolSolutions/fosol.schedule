using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    [Produces("application/json")]
    [Area("api")]
    [Route("[area]")]
    public class ApiController : Controller
    {
        #region Methods
        [HttpGet("endpoints")]
        public IActionResult Endpoints()
        {
            return Ok(GetEndpoints());
        }
        #endregion

        #region Private Methods
        private IEnumerable<object> GetEndpoints()
        {
            return Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.IsSubclassOf(typeof(Controller))).Select(t => new
            {
                Name = GetControllerName(t),
                Endpoints = t.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Select(mi => new
                {
                    mi.Name,
                    Routes = GetRoutes(t, mi)
                })
            });
        }

        private string GetControllerName(Type type)
        {
            return type.Name.Replace("Controller", "");
        }

        private IEnumerable<string> GetRoutes(Type type)
        {
            var controller = GetControllerName(type);
            var area = type.GetCustomAttribute<AreaAttribute>()?.RouteValue;
            return type.GetCustomAttributes<RouteAttribute>().Select(ra => ra.Template.Replace("[area]", area).Replace("[controller]", controller).ToLower()).Distinct();
        }

        private IEnumerable<string> GetRoutes(Type type, MethodInfo methodInfo)
        {
            var controller = GetControllerName(type);
            var area = type.GetCustomAttribute<AreaAttribute>()?.RouteValue;
            var controller_routes = GetRoutes(type);
            var endpoint_routes = methodInfo.GetCustomAttributes<RouteAttribute>().Select(ra => ra.Template.Replace("[area]", area).Replace("[controller]", controller).ToLower());
            var http_methods = methodInfo.GetCustomAttributes<HttpMethodAttribute>().Select(hga => hga.Template?.Replace("[area]", area).Replace("[controller]", controller).ToLower() ?? methodInfo.Name.ToLower());
            return endpoint_routes.Concat(http_methods).Distinct().Select(r => r.StartsWith("/") ? new[] { r } : controller_routes.Select(cr => $"/{cr}/{r}")).SelectMany(r => r);
        }
        #endregion
    }
}