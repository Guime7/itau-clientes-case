using Itau.Case.Clientes.Application.Common.Mediator;

namespace Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;

public record DeletarClienteCommand(int Id) : IRequest<bool>;
