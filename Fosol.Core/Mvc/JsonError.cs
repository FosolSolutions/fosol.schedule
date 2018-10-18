using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Fosol.Core.Mvc
{
    public class JsonError
    {
        #region Properties
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        #endregion

        #region Constructors
        public JsonError()
        {

        }

        public JsonError(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public JsonError(HttpStatusCode statusCode, Exception exception)
        {
            this.StatusCode = statusCode;

#if DEBUG
            this.Message = exception.InnerMessages();
#else
            if (exception is NoContentException
                || exception is InvalidOperationException
                || exception is NotAuthenticatedException
                || exception is NotAuthorizedException)
            {
                this.Message = exception.Message;
            }
            else
            {
                this.Message = "An unhandled error occured.";
            }
#endif
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
