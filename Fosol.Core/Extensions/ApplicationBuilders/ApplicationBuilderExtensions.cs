using Fosol.Core.Exceptions;
using Fosol.Core.Mvc;
using Fosol.Core.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Fosol.Core.Extensions.ApplicationBuilders
{
    public static class ApplicationBuilderExtensions
    {
        #region Methods
        /// <summary>
        /// Using the default 'UseExceptionHandler' all unhandled exceptions will be returned as JSON.
        /// </summary>
        /// <param name="app"></param>
        public static void UseJsonExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var status = HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)status;

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionFeature != null)
                    {
                        await context.HandleExceptionResponse(exceptionFeature.Error);
                    }
                });
            });
        }

        /// <summary>
        /// Add the JsonException middleware to return all unhandled exceptions as JSON.
        /// </summary>
        /// <param name="app"></param>
        public static void UseJsonExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<JsonExceptionHandler>();
        }

        /// <summary>
        /// Generic implementation for converting unhandled exceptions into JSON.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal static Task HandleExceptionResponse(this HttpContext context, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;
            var logger = context.RequestServices.GetService<ILoggerFactory>()?.CreateLogger(exception.GetType().Name);
            var env = context.RequestServices.GetRequiredService<IHostingEnvironment>();
            var converter = context.RequestServices.GetRequiredService<JsonOutputFormatter>();
            logger?.LogError(exception, $"An error occured while executing {context.Request.Path}.");

            // Some exceptions are expected and should return their error message.
            JsonError error;
            if (exception is InvalidOperationException || exception is NoContentException)
            {
                status = HttpStatusCode.BadRequest;
                error = new JsonError(status, exception);
            }
            else if (exception is NotAuthenticatedException)
            {
                status = HttpStatusCode.Unauthorized;
                error = new JsonError(status, exception);
            }
            else if (exception is NotAuthorizedException)
            {
                status = HttpStatusCode.Forbidden;
                error = new JsonError(status, exception);
            }
            else
            {
                // Only include the full message in a development environment.
                if (!(env?.IsDevelopment() ?? false))
                {
                    error = new JsonError(status, "An unhandled error occured.");
                }
                else
                {
                    error = new JsonError(status, exception);
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            using (var writer = new StringWriter())
            {
                converter.WriteObject(writer, error);
                var response = writer.ToString();
                return context.Response.WriteAsync(response);
            }
        }
        #endregion
    }
}
