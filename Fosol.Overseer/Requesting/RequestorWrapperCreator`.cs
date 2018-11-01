using Fosol.Overseer.Triggers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Requesting
{
    /// <summary>
    /// Represents an async continuation for the next task to execute in the pipeline
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <returns>Awaitable task returning a <typeparamref name="TResponse"/></returns>
    public delegate Task<TResponse> RequestorDelegate<TResponse>();

    /// <summary>
    /// Execute the pre-trigger-requestor, requestor and the post-trigger-requestor.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class RequestorWrapperCreator<TRequest, TResponse> : RequestorWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Execute all of the requestors for the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="serviceFactory"></param>
        /// <returns></returns>
        public override Task<TResponse> Execute(IRequest<TResponse> request, CancellationToken cancellationToken, ServiceFactory serviceFactory)
        {
            Task<TResponse> Requestor() => GetRequestor<IRequestor<TRequest, TResponse>>(serviceFactory).Execute((TRequest)request, cancellationToken);

            return serviceFactory
                .GetInstances<IRequestTrigger<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestorDelegate<TResponse>)Requestor, (next, pipeline) => () => pipeline.Execute((TRequest)request, cancellationToken, next))();
        }
    }
}
