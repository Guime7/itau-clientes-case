using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class DeletarClienteHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly DeletarClienteHandler _handler;

    public DeletarClienteHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new DeletarClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveDeletarCliente_QuandoClienteExiste()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        var command = new DeletarClienteCommand(1);

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(x => x.RemoverAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoClienteNaoEncontrado()
    {
        // Arrange
        var command = new DeletarClienteCommand(999);

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cliente não encontrado.");
    }
}
