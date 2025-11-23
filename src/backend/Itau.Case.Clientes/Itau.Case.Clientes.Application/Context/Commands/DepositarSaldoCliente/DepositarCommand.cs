using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Common;

namespace Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;

public record DepositarCommand(
    int ClienteId,
    decimal Valor,
    string? Descricao = null)
    : IRequest<Result<DepositarCommandResult>>;

public record DepositarCommandResult(
    int ClienteId,
    decimal SaldoAnterior,
    decimal Valor,
    decimal SaldoAtual);
