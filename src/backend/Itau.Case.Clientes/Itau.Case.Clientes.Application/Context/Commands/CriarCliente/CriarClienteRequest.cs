namespace Itau.Case.Clientes.Application.Context.Commands.CriarCliente;

public record CriarClienteRequest(
    string Nome,
    string Email);
