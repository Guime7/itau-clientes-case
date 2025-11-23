using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Common;

namespace Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;

public record DeletarClienteCommand(int Id) : IRequest<Result<bool>>;
