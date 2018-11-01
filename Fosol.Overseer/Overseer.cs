using Fosol.Overseer.Publishing;
using Fosol.Overseer.Requesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer
{
    public class Overseer : IOverseer
    {
        #region Variables
        private readonly ServiceFactory _serviceFactory;
        private static readonly ConcurrentDictionary<Type, object> _requestors = new ConcurrentDictionary<Type, object>();
        #endregion

        #region Constructors
        public Overseer(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send the request to the requestors, which will include all included triggers.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
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

        public Task<TResponse> Send<TRequestor, TRequest, TResponse>(TRequest request, Expression<Func<TRequestor, Func<TRequest, CancellationToken, Task<TResponse>>>> path, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();

            // Get or add the requestor to the inmemory dictionary.
            var requestor = (RequestorWrapper<TResponse>)_requestors.GetOrAdd(requestType,
                t => Activator.CreateInstance(typeof(RequestorWrapperCreator<,>).MakeGenericType(requestType, typeof(TResponse))));

            var call = path.Compile();
            return null;
            //return call.Invoke((TRequestor)requestor.Requestor)?.Invoke(request, cancellationToken);
        }
        #endregion
    }


}
