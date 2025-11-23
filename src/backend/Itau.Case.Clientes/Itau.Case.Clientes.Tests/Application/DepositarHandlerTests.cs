using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class DepositarHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly DepositarHandler _handler;

    public DepositarHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new DepositarHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveDepositarComSucesso()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        var command = new DepositarCommand(1, 100m, "Depósito inicial");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.ClienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaldoAnterior.Should().Be(0);
        result.Valor.Should().Be(100m);
        result.SaldoAtual.Should().Be(100m);
        _mockRepository.Verify(x => x.AtualizarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoClienteNaoEncontrado()
    {
        // Arrange
        var command = new DepositarCommand(999, 100m, "Depósito");

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
