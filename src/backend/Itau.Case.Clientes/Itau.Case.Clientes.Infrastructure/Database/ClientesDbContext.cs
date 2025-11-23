using Itau.Case.Clientes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Itau.Case.Clientes.Infrastructure.Data;

public class ClientesDbContext : DbContext
{
    private static SqliteConnection? _connection;

    public ClientesDbContext(DbContextOptions<ClientesDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public static SqliteConnection GetInMemoryConnection()
    {
        if (_connection == null)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }
        return _connection;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.Saldo)
                .HasPrecision(18, 2);

            entity.Property(e => e.DataCriacao)
                .IsRequired();

            entity.Property(e => e.DataAtualizacao)
                .IsRequired();
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ClienteId)
                .IsRequired();

            entity.Property(e => e.Tipo)
                .IsRequired();

            entity.Property(e => e.Valor)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(e => e.Descricao)
                .HasMaxLength(500);

            entity.Property(e => e.DataTransacao)
                .IsRequired();
        });
    }
}
