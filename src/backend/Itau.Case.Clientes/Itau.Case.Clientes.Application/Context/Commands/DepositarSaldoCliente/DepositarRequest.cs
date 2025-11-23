namespace Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;

public record DepositarRequest(
    decimal Valor,
    string? Descricao = null);
