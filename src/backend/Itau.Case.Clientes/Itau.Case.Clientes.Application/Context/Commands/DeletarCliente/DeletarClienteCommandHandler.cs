using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;

namespace Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;

public class DeletarClienteHandler(IClienteRepository clienteRepository) : IHandler<DeletarClienteCommand, bool>
{
    public async Task<bool> Handle(DeletarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
        if (cliente == null)
            throw new InvalidOperationException("Cliente não encontrado.");

        await clienteRepository.RemoverAsync(request.Id, cancellationToken);
        return true;
    }
}
