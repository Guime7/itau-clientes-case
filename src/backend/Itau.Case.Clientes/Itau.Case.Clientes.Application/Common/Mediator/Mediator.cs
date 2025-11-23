using System.Diagnostics.CodeAnalysis;

namespace Itau.Case.Clientes.Application.Common.Mediator;

[ExcludeFromCodeCoverage]
public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType) ?? throw new Exception($"Nenhum handler encontrado para {request.GetType().Name}");

        var handleMethod = handlerType.GetMethod("Handle");
        return (Task<TResponse>)handleMethod!.Invoke(handler, [request, cancellationToken])!;
    }
}