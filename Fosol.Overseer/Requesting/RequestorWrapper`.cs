using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Requesting
{
    internal abstract class RequestorWrapper<TResponse> : RequestorBase
    {
        #region Methods
        public abstract Task<TResponse> Execute(IRequest<TResponse> request, CancellationToken cancellationToken, ServiceFactory serviceFactory);

        public abstract Task<TResponse> Execute<TRequestor, TRequest>(TRequest request, Expression<Func<TRequestor, Func<TRequest, CancellationToken, Task<TResponse>>>> caller, CancellationToken cancellationToken, ServiceFactory serviceFactory)
            where TRequest : IRequest<TResponse>;
        #endregion
    }
}
