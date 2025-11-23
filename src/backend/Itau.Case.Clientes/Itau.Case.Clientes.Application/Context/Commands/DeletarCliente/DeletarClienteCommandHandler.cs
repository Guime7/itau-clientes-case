using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;

public class DeletarClienteHandler(IClienteRepository clienteRepository, ILogger<DeletarClienteHandler> logger) 
    : IHandler<DeletarClienteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletarClienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Deletando cliente. Id: {ClienteId}", request.Id);

            var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
            if (cliente == null)
            {
                logger.LogWarning("Cliente não encontrado para deleção. Id: {ClienteId}", request.Id);
                return Error.NotFound("Cliente não encontrado.");
            }

            await clienteRepository.RemoverAsync(request.Id, cancellationToken);

            logger.LogInformation("Cliente deletado com sucesso. Id: {ClienteId}", request.Id);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao deletar cliente. Id: {ClienteId}", request.Id);
            return Error.Failure("Erro ao deletar cliente.");
        }
    }
}
