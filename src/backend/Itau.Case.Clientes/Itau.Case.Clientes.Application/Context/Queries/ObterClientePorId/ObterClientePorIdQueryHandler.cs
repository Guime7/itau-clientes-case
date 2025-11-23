using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;

public class ObterClientePorIdQueryHandler(IClienteRepository clienteRepository) : IHandler<ObterClientePorIdQuery, ClienteDto?>
{
    public async Task<ClienteDto?> Handle(ObterClientePorIdQuery request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
        if (cliente == null)
            return null;

        return new ClienteDto(
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Saldo,
            cliente.DataCriacao,
            cliente.DataAtualizacao);
    }
}
