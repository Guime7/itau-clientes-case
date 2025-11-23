using Itau.Case.Clientes.Application.Common.Mediator;

namespace Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;

public record SacarCommand(
    int ClienteId,
    decimal Valor,
    string? Descricao = null)
    : IRequest<SacarCommandResult>;

public record SacarCommandResult(
    int ClienteId,
    decimal SaldoAnterior,
    decimal Valor,
    decimal SaldoAtual);
