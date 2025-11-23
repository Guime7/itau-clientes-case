using FluentAssertions;
using Itau.Case.Clientes.Domain.Entities;
using Itau.Case.Clientes.Domain.Enums;
using Itau.Case.Clientes.Domain.Exceptions;

namespace Itau.Case.Clientes.UnitTests.Domain;

public class ClienteTests
{
    [Fact]
    public void Cliente_DeveCriarComSucesso()
    {
        // Arrange & Act
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Assert
        cliente.Nome.Should().Be("João Silva");
        cliente.Email.Should().Be("joao@email.com");
        cliente.Saldo.Should().Be(0);
    }

    [Fact]
    public void Cliente_DeveLancarExcecao_QuandoNomeVazio()
    {
        // Arrange & Act
        Action act = () => new Cliente("", "joao@email.com");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void Cliente_DeveLancarExcecao_QuandoEmailVazio()
    {
        // Arrange & Act
        Action act = () => new Cliente("João Silva", "");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O email não pode ser vazio.");
    }

    [Fact]
    public void Cliente_DeveLancarExcecao_QuandoEmailInvalido()
    {
        // Arrange & Act
        Action act = () => new Cliente("João Silva", "email-invalido");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Formato de email inválido.");
    }

    [Fact]
    public void AtualizarNome_DeveAtualizarComSucesso()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        cliente.AtualizarNome("José Silva");

        // Assert
        cliente.Nome.Should().Be("José Silva");
    }

    [Fact]
    public void AtualizarNome_DeveLancarExcecao_QuandoNomeVazio()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        Action act = () => cliente.AtualizarNome("");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void AtualizarEmail_DeveAtualizarComSucesso()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        cliente.AtualizarEmail("jose@email.com");

        // Assert
        cliente.Email.Should().Be("jose@email.com");
    }

    [Fact]
    public void AtualizarEmail_DeveLancarExcecao_QuandoEmailInvalido()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        Action act = () => cliente.AtualizarEmail("email-invalido");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Formato de email inválido.");
    }

    [Fact]
    public void Depositar_DeveAumentarSaldo()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        var transacao = cliente.Depositar(100.50m, "Depósito inicial");

        // Assert
        cliente.Saldo.Should().Be(100.50m);
        transacao.Tipo.Should().Be(ETipoTransacao.Deposito);
        transacao.Valor.Should().Be(100.50m);
    }

    [Fact]
    public void Depositar_DeveLancarExcecao_QuandoValorNegativo()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        Action act = () => cliente.Depositar(-10);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor inválido.");
    }

    [Fact]
    public void Sacar_DeveDiminuirSaldo()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        cliente.Depositar(100);

        // Act
        var transacao = cliente.Sacar(50, "Saque");

        // Assert
        cliente.Saldo.Should().Be(50);
        transacao.Tipo.Should().Be(ETipoTransacao.Saque);
        transacao.Valor.Should().Be(50);
    }

    [Fact]
    public void Sacar_DeveLancarExcecao_QuandoSaldoInsuficiente()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        cliente.Depositar(50);

        // Act
        Action act = () => cliente.Sacar(100);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Saldo insuficiente.");
    }

    [Fact]
    public void Sacar_DeveLancarExcecao_QuandoValorNegativo()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");

        // Act
        Action act = () => cliente.Sacar(-10);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor inválido.");
    }
}
