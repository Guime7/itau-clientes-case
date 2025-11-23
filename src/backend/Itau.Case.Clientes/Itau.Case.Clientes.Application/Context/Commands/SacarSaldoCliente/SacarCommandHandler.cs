using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;

namespace Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;

public class SacarHandler(IClienteRepository clienteRepository) : IHandler<SacarCommand, SacarCommandResult>
{
    public async Task<SacarCommandResult> Handle(SacarCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken);
        if (cliente == null)
            throw new InvalidOperationException("Cliente não encontrado.");

        var saldoAnterior = cliente.Saldo;
        cliente.Sacar(request.Valor, request.Descricao);

        await clienteRepository.AtualizarAsync(cliente, cancellationToken);

        return new SacarCommandResult(
            cliente.Id,
            saldoAnterior,
            request.Valor,
            cliente.Saldo);
    }
}
