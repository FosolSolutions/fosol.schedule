using Fosol.Overseer.Requesting;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Triggers
{
    internal interface IRequestTrigger<in TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Pipeline handler. Perform any additional behavior and await the <paramref name="next"/> delegate as necessary
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/></returns>
        Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken, RequestorDelegate<TResponse> next);
    }
}