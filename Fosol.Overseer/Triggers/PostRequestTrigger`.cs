using Fosol.Overseer.Requesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Triggers
{
    /// <summary>
    /// Behavior for executing all <see cref="IPostRequestTrigger{TRequest,TResponse}"/> instances after handling the request
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    class PostRequestTrigger<TRequest, TResponse> : IRequestTrigger<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        #region Variables
        private readonly IEnumerable<IPostRequestTrigger<TRequest, TResponse>> _postTriggers;
        #endregion

        #region Constructors
        public PostRequestTrigger(IEnumerable<IPostRequestTrigger<TRequest, TResponse>> postTriggers)
        {
            _postTriggers = postTriggers;
        }
        #endregion

        #region Methods
        public async Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken, RequestorDelegate<TResponse> next)
        {
            var response = await next().ConfigureAwait(false);

            foreach (var processor in _postTriggers)
            {
                await processor.Process(request, response).ConfigureAwait(false);
            }

            return response;
        }
        #endregion
    }
}
