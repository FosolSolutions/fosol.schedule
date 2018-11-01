using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Requesting
{
    public interface IRequestor<in TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
    }
}