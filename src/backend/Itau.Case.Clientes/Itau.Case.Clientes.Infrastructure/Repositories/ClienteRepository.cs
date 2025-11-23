using Dapper;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Itau.Case.Clientes.Infrastructure.Data;
using System.Reflection;

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
        const string sql = @"
            INSERT INTO clientes (Nome, Email, Saldo, DataCriacao, DataAtualizacao)
            VALUES (@Nome, @Email, @Saldo, @DataCriacao, @DataAtualizacao);
            SELECT LAST_INSERT_ID();";

        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(sql, new
        {
            cliente.Nome,
            cliente.Email,
            cliente.Saldo,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        });

        SetPrivateProperty(cliente, "Id", id);
        return cliente;
    }

    public async Task AtualizarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE clientes 
            SET Nome = @Nome, Email = @Email, Saldo = @Saldo, DataAtualizacao = @DataAtualizacao
            WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Saldo,
            DataAtualizacao = DateTime.UtcNow
        });
    }

    public async Task<bool> ExisteEmailAsync(string email, int? clienteIdParaIgnorar = null, CancellationToken cancellationToken = default)
    {
        var sql = "SELECT COUNT(1) FROM clientes WHERE Email = @Email";
        
        if (clienteIdParaIgnorar.HasValue)
        {
            sql += " AND Id != @ClienteId";
        }

        using var connection = _context.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email, ClienteId = clienteIdParaIgnorar });
        return count > 0;
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM clientes WHERE Email = @Email";
        
        using var connection = _context.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Email = email });
        
        return result != null ? MapToCliente(result) : null;
    }

    public async Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM clientes WHERE Id = @Id";
        
        using var connection = _context.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
        
        return result != null ? MapToCliente(result) : null;
    }

    public async Task<IEnumerable<Cliente>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM clientes";
        
        using var connection = _context.CreateConnection();
        var results = await connection.QueryAsync<dynamic>(sql);
        
        return results.Select(MapToCliente);
    }

    public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM clientes WHERE Id = @Id";
        
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    private static Cliente MapToCliente(dynamic row)
    {
        var cliente = (Cliente)Activator.CreateInstance(typeof(Cliente), true)!;
        
        SetPrivateProperty(cliente, "Id", (int)row.Id);
        SetPrivateProperty(cliente, "Nome", (string)row.Nome);
        SetPrivateProperty(cliente, "Email", (string)row.Email);
        SetPrivateProperty(cliente, "Saldo", (decimal)row.Saldo);
        SetPrivateProperty(cliente, "DataCriacao", (DateTime)row.DataCriacao);
        SetPrivateProperty(cliente, "DataAtualizacao", (DateTime)row.DataAtualizacao);
        
        return cliente;
    }

    private static void SetPrivateProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        property?.SetValue(obj, value);
    }
}
