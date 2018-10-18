﻿using Fosol.Core.Extensions.ApplicationBuilders;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fosol.Core.Mvc.Filters
{
    /// <summary>
    /// JsonExceptionFilterAttribute class, provides a way to create a JSON response for unhandled exceptions.
    /// </summary>
    public class JsonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a JsonExceptionFilterAttribute object.
        /// </summary>
        public JsonExceptionFilterAttribute()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// When an exception occurs it will create a JSON response with an appropriate status code.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.HandleExceptionResponse(context.Exception);
            context.ExceptionHandled = true;
            base.OnException(context);
        }
        #endregion
    }
}
