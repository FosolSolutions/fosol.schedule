using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// <typeparamref name="ApiController" class, provides API endpoints that describe all other endpoints in this application.
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
            return Ok(GetEndpoints());
        }
        #endregion

        #region Private Methods
        private IEnumerable<object> GetEndpoints()
        {
            var xml = new XPathDocument($@"{AppDomain.CurrentDomain.BaseDirectory}/{typeof(Program).Assembly.GetName().Name}.xml");
            var nav = xml.CreateNavigator();

            return Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.IsSubclassOf(typeof(Controller))).Select(t => new
            {
                Name = GetControllerName(t),
                Endpoints = t.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly).Select(mi => new
                {
                    mi.Name,
                    Summary = GetValue(nav, $"{GetMemberPath(t, mi)}/summary"),
                    Routes = GetRoutes(t, mi),
                    Parameters = GetParameters(t, mi, nav)
                })
            });
        }

        private string GetValue(XPathNavigator nav, string xpath)
        {
            return nav.SelectSingleNode(xpath)?.Value.Trim();
        }

        private string GetMemberPath(Type type, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0)
                return $"/doc/members/member[@name='M:{type.FullName}.{methodInfo.Name}']";

            var paramPath = $"({String.Join(",", parameters.Select(p => GetParameterType(p)))})";
            return $"/doc/members/member[@name='M:{type.FullName}.{methodInfo.Name}{paramPath}']";
        }

        private string GetParameterType(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.ParameterType;
            if (IsNullable(type))
                return $"System.Nullable{{{type.GetGenericArguments()[0]}}}";

            return type.FullName;
        }

        private bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
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

        private IEnumerable<object> GetParameters(Type type, MethodInfo methodInfo, XPathNavigator nav)
        {
            return methodInfo.GetParameters().Select(p => new
            {
                p.Name,
                Type = GetParameterType(p),
                Summary = GetValue(nav, $"/param[@name='{p.Name}']")
            });
        }
        #endregion
    }
}