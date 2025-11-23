using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;

public class ObterTodosClientesQueryHandler(IClienteRepository clienteRepository) : IHandler<ObterTodosClientesQuery, IEnumerable<ClienteDto>>
{
    public async Task<IEnumerable<ClienteDto>> Handle(ObterTodosClientesQuery request, CancellationToken cancellationToken)
    {
        var clientes = await clienteRepository.ObterTodosAsync(cancellationToken);

        return clientes.Select(c => new ClienteDto(
            c.Id,
            c.Nome,
            c.Email,
            c.Saldo,
            c.DataCriacao,
            c.DataAtualizacao));
    }
}
