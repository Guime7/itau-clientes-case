using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;

public class AtualizarClienteHandler(IClienteRepository clienteRepository) : IHandler<AtualizarClienteCommand, ClienteDto>
{
    public async Task<ClienteDto> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
        if (cliente == null)
            throw new InvalidOperationException("Cliente não encontrado.");

        var emailExiste = await clienteRepository.ExisteEmailAsync(request.Email, request.Id, cancellationToken);
        if (emailExiste)
            throw new InvalidOperationException("Já existe outro cliente cadastrado com este email.");

        cliente.AtualizarNome(request.Nome);
        cliente.AtualizarEmail(request.Email);

        await clienteRepository.AtualizarAsync(cliente, cancellationToken);

        return new ClienteDto(
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Saldo,
            cliente.DataCriacao,
            cliente.DataAtualizacao);
    }
}
