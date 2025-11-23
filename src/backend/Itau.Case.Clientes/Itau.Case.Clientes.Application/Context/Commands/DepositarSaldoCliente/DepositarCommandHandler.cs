using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;

public class DepositarHandler(IClienteRepository clienteRepository, ILogger<DepositarHandler> logger) 
    : IHandler<DepositarCommand, Result<DepositarCommandResult>>
{
    public async Task<Result<DepositarCommandResult>> Handle(DepositarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Processando depósito. ClienteId: {ClienteId}, Valor: {Valor}", request.ClienteId, request.Valor);

            var cliente = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken);
            if (cliente == null)
            {
                logger.LogWarning("Cliente não encontrado para depósito. ClienteId: {ClienteId}", request.ClienteId);
                return Error.NotFound("Cliente não encontrado.");
            }

            var saldoAnterior = cliente.Saldo;
            var transacao = cliente.Depositar(request.Valor, request.Descricao);

            await clienteRepository.AtualizarComTransacaoAsync(cliente, transacao, cancellationToken);

            logger.LogInformation("Depósito realizado com sucesso. ClienteId: {ClienteId}, SaldoAnterior: {SaldoAnterior}, SaldoAtual: {SaldoAtual}", 
                cliente.Id, saldoAnterior, cliente.Saldo);

            var result = new DepositarCommandResult(
                cliente.Id,
                saldoAnterior,
                request.Valor,
                cliente.Saldo);

            return Result<DepositarCommandResult>.Success(result);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Erro de validação ao processar depósito. ClienteId: {ClienteId}", request.ClienteId);
            return Error.Validation(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar depósito. ClienteId: {ClienteId}", request.ClienteId);
            return Error.Failure("Erro ao processar depósito.");
        }
    }
}
