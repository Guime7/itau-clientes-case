using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;

public record ObterTodosClientesQuery : IRequest<IEnumerable<ClienteDto>>;
