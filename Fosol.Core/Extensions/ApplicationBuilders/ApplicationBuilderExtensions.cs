using Fosol.Core.Exceptions;
using Fosol.Core.Mvc;
using Fosol.Core.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fosol.Core.Extensions.ApplicationBuilders
{
    public static class ApplicationBuilderExtensions
    {
        #region Methods
        public static void UseJsonExceptionHandler(this IApplicationBuilder app, ILogger logger = null)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var status = HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)status;
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionFeature != null)
                    {
                        await context.HandleExceptionResponse(exceptionFeature.Error, logger);
                    }
                });
            });
        }

        public static void UseJsonExceptionMiddleware(this IApplicationBuilder app, ILogger logger = null)
        {
            app.UseMiddleware<JsonExceptionHandler>();
        }

        internal static Task HandleExceptionResponse(this HttpContext context, Exception exception, ILogger logger = null)
        {
            var status = HttpStatusCode.InternalServerError;
            logger?.LogError(exception, $"An error occured while executing {context.Request.Path}.");

            if (exception is InvalidOperationException || exception is NoContentException)
            {
                status = HttpStatusCode.BadRequest;
            }
            else if (exception is NotAuthenticatedException)
            {
                status = HttpStatusCode.Unauthorized;
            }
            else if (exception is NotAuthorizedException)
            {
                status = HttpStatusCode.Forbidden;
            }

            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(new JsonError(status, exception).ToString());
        }
        #endregion
    }
}
