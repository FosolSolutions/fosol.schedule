using Fosol.Overseer.Requesting;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer
{
    public interface IOverseer
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

        Task<TResponse> Send<TRequestor, TRequest, TResponse>(TRequest request, Expression<Func<TRequestor, Func<TRequest, CancellationToken, Task<TResponse>>>> path, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>;
    }
}