using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;

public class ObterTodosClientesQueryHandler(IClienteRepository clienteRepository, ILogger<ObterTodosClientesQueryHandler> logger) 
    : IHandler<ObterTodosClientesQuery, Result<IEnumerable<ClienteDto>>>
{
    public async Task<Result<IEnumerable<ClienteDto>>> Handle(ObterTodosClientesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Obtendo todos os clientes");

            var clientes = await clienteRepository.ObterTodosAsync(cancellationToken);

            var dtos = clientes.Select(c => new ClienteDto(
                c.Id,
                c.Nome,
                c.Email,
                c.Saldo,
                c.DataCriacao,
                c.DataAtualizacao));

            logger.LogInformation("Total de clientes encontrados: {Total}", dtos.Count());

            return Result<IEnumerable<ClienteDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todos os clientes");
            return Error.Failure("Erro ao obter clientes.");
        }
    }
}
