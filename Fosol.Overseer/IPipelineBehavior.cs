namespace Fosol.Overseer
{
    internal interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}