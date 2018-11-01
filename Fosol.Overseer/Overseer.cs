using Fosol.Overseer.Requesting;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer
{
    /// <summary>
    /// Overseer class, provides a way to manage the request pipeline.
    /// </summary>
    public class Overseer : IOverseer
    {
        #region Variables
        private readonly ServiceFactory _serviceFactory;
        private static readonly ConcurrentDictionary<Type, object> _requestors = new ConcurrentDictionary<Type, object>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Overseer object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="serviceFactory"></param>
        public Overseer(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send the request to the requestors, which will include all injected triggers.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();

            // Get or add the requestor to the inmemory dictionary.
            var requestor = (RequestorWrapper<TResponse>)_requestors.GetOrAdd(requestType,
                t => Activator.CreateInstance(typeof(RequestorWrapperCreator<,>).MakeGenericType(requestType, typeof(TResponse))));

            return requestor.Execute(request, cancellationToken, _serviceFactory);
        }

        /// <summary>
        /// Send the request to the requestors, which will include all injected triggers.
        /// Instead of the default Execute(...) function, call the specified caller.
        /// </summary>
        /// <typeparam name="TRequestor"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="caller">The function to call instead of the default Execute(...) function.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> Send<TRequestor, TRequest, TResponse>(TRequest request, Expression<Func<TRequestor, Func<TRequest, CancellationToken, Task<TResponse>>>> caller, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
            where TRequestor : IRequestor<TRequest, TResponse>
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();

            // Get or add the requestor to the inmemory dictionary.
            var requestor = (RequestorWrapper<TResponse>)_requestors.GetOrAdd(requestType,
                t => Activator.CreateInstance(typeof(RequestorWrapperCreator<,>).MakeGenericType(requestType, typeof(TResponse))));

            return requestor.Execute(request, caller, cancellationToken, _serviceFactory);
        }

        /// <summary>
        /// Send the request to the requestors, which will include all injected triggers.
        /// Execute the preTask and postTask functions.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="preTask">A pre-task to perform before executing the request.</param>
        /// <param name="postTask">A post-task to perform after executing the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, Func<IRequest<TResponse>, CancellationToken, IRequest<TResponse>> preTask, Func<Task<TResponse>, CancellationToken, Task<TResponse>> postTask, CancellationToken cancellationToken = default)
        {
            var newRequest = preTask?.Invoke(request, cancellationToken);
            var response = this.Send(newRequest, cancellationToken);
            postTask?.Invoke(response, cancellationToken);

            return response;
        }

        /// <summary>
        /// Send the request to the requestors, which will include all injected triggers.
        /// Execute the preTask and postTask functions.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="preTask">A pre-task to perform before executing the request.</param>
        /// <param name="postTask">A post-task to perform after executing the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, Func<IRequest<TResponse>, IRequest<TResponse>> preTask, Func<Task<TResponse>, Task<TResponse>> postTask, CancellationToken cancellationToken = default)
        {
            var newRequest = preTask?.Invoke(request);
            var response = this.Send(newRequest, cancellationToken);
            postTask?.Invoke(response);

            return response;
        }
        #endregion
    }


}
