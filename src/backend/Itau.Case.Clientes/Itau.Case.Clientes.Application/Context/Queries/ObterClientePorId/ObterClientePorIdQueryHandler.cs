using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;

public class ObterClientePorIdQueryHandler(IClienteRepository clienteRepository, ILogger<ObterClientePorIdQueryHandler> logger) 
    : IHandler<ObterClientePorIdQuery, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> Handle(ObterClientePorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Obtendo cliente por Id. Id: {ClienteId}", request.Id);

            var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
            
            if (cliente == null)
            {
                logger.LogWarning("Cliente não encontrado. Id: {ClienteId}", request.Id);
                return Error.NotFound("Cliente não encontrado.");
            }

            var dto = new ClienteDto(
                cliente.Id,
                cliente.Nome,
                cliente.Email,
                cliente.Saldo,
                cliente.DataCriacao,
                cliente.DataAtualizacao);

            return Result<ClienteDto>.Success(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter cliente. Id: {ClienteId}", request.Id);
            return Error.Failure("Erro ao obter cliente.");
        }
    }
}
