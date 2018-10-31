namespace Fosol.Overseer
{
    internal interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}