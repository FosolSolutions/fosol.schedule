using System;
using System.Text;

namespace Fosol.Core.Extensions.Exceptions
{
    /// <summary>
    /// ExceptionExtensions static class, provides extension methods for Exception objects.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Methods
        /// <summary>
        /// Appends all messages found within the exception, which includes the inner exceptions.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string InnerMessages(this Exception exception)
        {
            var message = new StringBuilder();
            message.Append(exception.Message);
            var ex = exception;
            while (ex.InnerException != null)
            {
                message.AppendLine($"{ex.InnerException.Message} ");
                ex = ex.InnerException;
            }
            return message.ToString();
        }
        #endregion
    }
}
