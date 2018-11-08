using Fosol.Core.Extensions.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Fosol.Core.Mvc
{
    public class JsonErrorHandler
    {
        #region Properties
        public JsonSerializerSettings Settings { get; }
        public IHostingEnvironment Environment { get; }
        #endregion

        #region Constructors
        public JsonErrorHandler(IOptions<MvcJsonOptions> options, IHostingEnvironment environment)
        {
            this.Settings = options?.Value?.SerializerSettings ?? throw new ArgumentNullException(nameof(options));
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
        #endregion

        #region Methods
        public JsonError Wrap(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            if (this.Environment.IsDevelopment())
            {
                return new JsonError(statusCode, exception.InnerMessages());
            }
            else
            {
                return new JsonError(statusCode, exception);
            }
        }

        public string Serialize(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return JsonConvert.SerializeObject(Wrap(exception, statusCode), this.Settings);
        }
        #endregion
    }
}
