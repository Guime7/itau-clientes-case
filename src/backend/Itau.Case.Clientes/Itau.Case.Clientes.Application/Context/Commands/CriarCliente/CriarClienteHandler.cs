using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Dtos;
using Itau.Case.Clientes.Domain.Entities;

namespace Itau.Case.Clientes.Application.Context.Commands.CriarCliente;

public class CriarClienteHandler(IClienteRepository clienteRepository) : IHandler<CriarClienteCommand, ClienteDto>
{
     public async Task<ClienteDto> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var emailExiste = await clienteRepository.ExisteEmailAsync(request.Email, null, cancellationToken);
        if (emailExiste)
            throw new InvalidOperationException("Já existe um cliente cadastrado com este email.");

        var novoCliente = new Cliente(request.Nome, request.Email);
        
        var clienteCriado = await clienteRepository.AdicionarAsync(novoCliente, cancellationToken);

        return new ClienteDto(
            clienteCriado.Id, 
            clienteCriado.Nome, 
            clienteCriado.Email, 
            clienteCriado.Saldo, 
            clienteCriado.DataCriacao, 
            clienteCriado.DataAtualizacao);
    }
}
