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

        Task<TResponse> Send<TRequestor, TRequest, TResponse>(TRequest request, Expression<Func<TRequestor, Func<TRequest, CancellationToken, Task<TResponse>>>> caller, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>;

        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, Func<IRequest<TResponse>, CancellationToken, IRequest<TResponse>> preTask, Func<Task<TResponse>, CancellationToken, Task<TResponse>> postTask, CancellationToken cancellationToken = default);

        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, Func<IRequest<TResponse>, IRequest<TResponse>> preTask, Func<Task<TResponse>, Task<TResponse>> postTask, CancellationToken cancellationToken = default);
    }
}