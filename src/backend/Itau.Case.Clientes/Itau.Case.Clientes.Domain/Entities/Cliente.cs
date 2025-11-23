using Itau.Case.Clientes.Domain.Base;
using Itau.Case.Clientes.Domain.Enums;
using Itau.Case.Clientes.Domain.Exceptions;

namespace Itau.Case.Clientes.Domain.Entities;
public class Cliente : AggregateRoot
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public decimal Saldo { get; private set; }
    public DateTime DataCriacao { get; protected set; }
    public DateTime DataAtualizacao { get; protected set; }

    // Construtor sem parâmetros para o EF Core
    protected Cliente() { }

    public Cliente(string nome, string email)
    {
        ValidarNome(nome);
        ValidarEmail(email);

        Nome = nome;
        Email = email;
        Saldo = 0;
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarNome(string nome)
    {
        ValidarNome(nome);
        Nome = nome;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarEmail(string email)
    {
        ValidarEmail(email);
        Email = email;
        DataAtualizacao = DateTime.UtcNow;
    }

    public Transacao Depositar(decimal valor, string? descricao = null)
    {
        DomainException.When(valor <= 0, "Valor inválido.");

        Saldo += valor;

        return new Transacao(ETipoTransacao.Deposito, valor, descricao);
    }

    public Transacao Sacar(decimal valor, string? descricao = null)
    {
        DomainException.When(valor <= 0, "Valor inválido.");
        DomainException.When(Saldo < valor, "Saldo insuficiente.");

        Saldo -= valor;
        DataAtualizacao = DateTime.UtcNow;

        return new Transacao(ETipoTransacao.Saque, valor, descricao);
    }

    private static void ValidarNome(string nome)
    {
        DomainException.When(string.IsNullOrWhiteSpace(nome), "O nome não pode ser vazio.");
    }

    private static void ValidarEmail(string email)
    {
        DomainException.When(string.IsNullOrWhiteSpace(email), "O email não pode ser vazio.");

        DomainException.When(!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"), "Formato de email inválido.");
    }
}
