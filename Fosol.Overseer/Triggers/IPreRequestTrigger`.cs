using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Overseer.Triggers
{
    public interface IPreRequestTrigger<in TRequest, in TResponse>
    {
        /// <summary>
        /// Process method executes after the Handle method on your handler
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="response">Response instance</param>
        /// <returns>An awaitable task</returns>
        Task Process(TRequest request, TResponse response);
    }
}
