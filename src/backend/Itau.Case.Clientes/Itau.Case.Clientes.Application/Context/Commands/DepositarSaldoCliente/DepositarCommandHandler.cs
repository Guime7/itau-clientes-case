using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;

namespace Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;

public class DepositarHandler(IClienteRepository clienteRepository) : IHandler<DepositarCommand, DepositarCommandResult>
{
    public async Task<DepositarCommandResult> Handle(DepositarCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken);
        if (cliente == null)
            throw new InvalidOperationException("Cliente não encontrado.");

        var saldoAnterior = cliente.Saldo;
        cliente.Depositar(request.Valor, request.Descricao);

        await clienteRepository.AtualizarAsync(cliente, cancellationToken);

        return new DepositarCommandResult(
            cliente.Id,
            saldoAnterior,
            request.Valor,
            cliente.Saldo);
    }
}
