using Fosol.Core.Extensions.ApplicationBuilders;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Fosol.Core.Mvc.Filters
{
    public class JsonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        #region Constructors
        public JsonExceptionFilterAttribute()
        {
        }
        #endregion

        #region Methods
        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger>();
            context.HttpContext.HandleExceptionResponse(context.Exception, logger);
            base.OnException(context);
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger>();
            context.HttpContext.HandleExceptionResponse(context.Exception, logger);
            return base.OnExceptionAsync(context);
        }
        #endregion
    }
}
