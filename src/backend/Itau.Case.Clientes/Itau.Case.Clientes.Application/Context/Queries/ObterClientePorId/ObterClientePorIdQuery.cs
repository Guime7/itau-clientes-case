using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Domain.Dtos;

namespace Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;

public record ObterClientePorIdQuery(int Id) : IRequest<ClienteDto?>;
