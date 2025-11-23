using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class SacarHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly SacarHandler _handler;

    public SacarHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new SacarHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveSacarComSucesso()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        cliente.Depositar(200m);
        var command = new SacarCommand(1, 50m, "Saque");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.ClienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaldoAnterior.Should().Be(200m);
        result.Valor.Should().Be(50m);
        result.SaldoAtual.Should().Be(150m);
        _mockRepository.Verify(x => x.AtualizarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoClienteNaoEncontrado()
    {
        // Arrange
        var command = new SacarCommand(999, 50m, "Saque");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.ClienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cliente não encontrado.");
    }
}
