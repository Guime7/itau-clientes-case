using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class AtualizarClienteHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly AtualizarClienteHandler _handler;

    public AtualizarClienteHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new AtualizarClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveAtualizarCliente_QuandoDadosValidos()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        var command = new AtualizarClienteCommand(1, "João Santos", "joao.santos@email.com");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        _mockRepository
            .Setup(x => x.ExisteEmailAsync(command.Email, command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João Santos");
        result.Email.Should().Be("joao.santos@email.com");
        _mockRepository.Verify(x => x.AtualizarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoClienteNaoEncontrado()
    {
        // Arrange
        var command = new AtualizarClienteCommand(999, "João Santos", "joao.santos@email.com");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cliente não encontrado.");
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoEmailJaExiste()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        var command = new AtualizarClienteCommand(1, "João Santos", "outro@email.com");

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        _mockRepository
            .Setup(x => x.ExisteEmailAsync(command.Email, command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe outro cliente cadastrado com este email.");
    }
}
