namespace Itau.Case.Clientes.Application.Common.Mediator;

public interface IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}