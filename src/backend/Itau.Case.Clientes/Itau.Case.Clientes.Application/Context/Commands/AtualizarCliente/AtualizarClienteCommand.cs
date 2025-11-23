using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;

public record AtualizarClienteCommand(
    int Id,
    string Nome,
    string Email)
    : IRequest<Result<ClienteDto>>;
