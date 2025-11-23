using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class CriarClienteHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly CriarClienteHandler _handler;

    public CriarClienteHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new CriarClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarCliente_QuandoDadosValidos()
    {
        // Arrange
        var command = new CriarClienteCommand("João Silva", "joao@email.com");
        var cliente = new Cliente("João Silva", "joao@email.com");

        _mockRepository
            .Setup(x => x.ExisteEmailAsync(command.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockRepository
            .Setup(x => x.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João Silva");
        result.Email.Should().Be("joao@email.com");
        _mockRepository.Verify(x => x.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoEmailJaExiste()
    {
        // Arrange
        var command = new CriarClienteCommand("João Silva", "joao@email.com");

        _mockRepository
            .Setup(x => x.ExisteEmailAsync(command.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe um cliente cadastrado com este email.");
    }
}
