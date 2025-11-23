using Itau.Case.Clientes.Domain.Entities;

namespace Itau.Case.Clientes.Application.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task AtualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task AtualizarComTransacaoAsync(Cliente cliente, Transacao transacao, CancellationToken cancellationToken = default);
    Task RemoverAsync(int id, CancellationToken cancellationToken = default);
    Task<Cliente> AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<bool> ExisteEmailAsync(string email, int? clienteIdParaIgnorar = null, CancellationToken cancellationToken = default);
}
