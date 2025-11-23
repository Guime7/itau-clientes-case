namespace Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;

public record SacarRequest(
    decimal Valor,
    string? Descricao = null);
