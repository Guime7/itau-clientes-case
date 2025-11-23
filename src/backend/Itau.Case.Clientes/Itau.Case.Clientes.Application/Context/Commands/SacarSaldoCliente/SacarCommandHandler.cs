using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;

public class SacarHandler(IClienteRepository clienteRepository, ILogger<SacarHandler> logger) 
    : IHandler<SacarCommand, Result<SacarCommandResult>>
{
    public async Task<Result<SacarCommandResult>> Handle(SacarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Processando saque. ClienteId: {ClienteId}, Valor: {Valor}", request.ClienteId, request.Valor);

            var cliente = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken);
            if (cliente == null)
            {
                logger.LogWarning("Cliente não encontrado para saque. ClienteId: {ClienteId}", request.ClienteId);
                return Error.NotFound("Cliente não encontrado.");
            }

            var saldoAnterior = cliente.Saldo;
            var transacao = cliente.Sacar(request.Valor, request.Descricao);

            await clienteRepository.AtualizarComTransacaoAsync(cliente, transacao, cancellationToken);

            logger.LogInformation("Saque realizado com sucesso. ClienteId: {ClienteId}, SaldoAnterior: {SaldoAnterior}, SaldoAtual: {SaldoAtual}", 
                cliente.Id, saldoAnterior, cliente.Saldo);

            var result = new SacarCommandResult(
                cliente.Id,
                saldoAnterior,
                request.Valor,
                cliente.Saldo);

            return Result<SacarCommandResult>.Success(result);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Erro de validação ao processar saque. ClienteId: {ClienteId}, Mensagem: {Mensagem}", request.ClienteId, ex.Message);
            return Error.Validation(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar saque. ClienteId: {ClienteId}", request.ClienteId);
            return Error.Failure("Erro ao processar saque.");
        }
    }
}
