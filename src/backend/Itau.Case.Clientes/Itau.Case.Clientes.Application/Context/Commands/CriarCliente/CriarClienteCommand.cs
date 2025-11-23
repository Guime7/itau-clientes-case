using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Commands.CriarCliente;

public record CriarClienteCommand(
    string Nome, 
    string Email)
    : IRequest<Result<ClienteDto>>;
