using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Itau.Case.Clientes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Itau.Case.Clientes.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesDbContext _context;

    public ClienteRepository(ClientesDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente> AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task AtualizarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? clienteIdParaIgnorar = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Clientes.Where(c => c.Email == email);

        if (clienteIdParaIgnorar.HasValue)
        {
            query = query.Where(c => c.Id != clienteIdParaIgnorar.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes.ToListAsync(cancellationToken);
    }

    public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await ObterPorIdAsync(id, cancellationToken);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
