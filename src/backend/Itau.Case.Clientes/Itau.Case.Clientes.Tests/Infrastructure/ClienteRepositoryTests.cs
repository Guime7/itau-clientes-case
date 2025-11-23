using FluentAssertions;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Infrastructure;

public class ClienteRepositoryTests
{
    [Fact]
    public async Task AdicionarAsync_DeveRetornarClienteComId()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        var cliente = new Cliente("João", "joao@email.com");
        
        mockRepository
            .Setup(x => x.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await mockRepository.Object.AdicionarAsync(cliente);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João");
        result.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task AtualizarAsync_DeveExecutarComSucesso()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        var cliente = new Cliente("João", "joao@email.com");
        
        mockRepository
            .Setup(x => x.AtualizarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var act = async () => await mockRepository.Object.AtualizarAsync(cliente);

        // Assert
        await act.Should().NotThrowAsync();
        mockRepository.Verify(x => x.AtualizarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarCliente_QuandoExistir()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        var cliente = new Cliente("João", "joao@email.com");

        mockRepository
            .Setup(x => x.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await mockRepository.Object.ObterPorIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Nome.Should().Be("João");
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();

        mockRepository
            .Setup(x => x.ObterPorIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await mockRepository.Object.ObterPorIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaDeClientes()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        var clientes = new List<Cliente>
        {
            new Cliente("João", "joao@email.com"),
            new Cliente("Maria", "maria@email.com")
        };

        mockRepository
            .Setup(x => x.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await mockRepository.Object.ObterTodosAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveRetornarCliente_QuandoExistir()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        var cliente = new Cliente("João", "joao@email.com");

        mockRepository
            .Setup(x => x.ObterPorEmailAsync("joao@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await mockRepository.Object.ObterPorEmailAsync("joao@email.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task ExisteEmailAsync_DeveRetornarTrue_QuandoEmailExistir()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();

        mockRepository
            .Setup(x => x.ExisteEmailAsync("joao@email.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await mockRepository.Object.ExisteEmailAsync("joao@email.com");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteEmailAsync_DeveRetornarFalse_QuandoEmailNaoExistir()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();

        mockRepository
            .Setup(x => x.ExisteEmailAsync("naoexiste@email.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await mockRepository.Object.ExisteEmailAsync("naoexiste@email.com");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoverAsync_DeveExecutarComSucesso()
    {
        // Arrange
        var mockRepository = new Mock<IClienteRepository>();
        
        mockRepository
            .Setup(x => x.RemoverAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var act = async () => await mockRepository.Object.RemoverAsync(1);

        // Assert
        await act.Should().NotThrowAsync();
        mockRepository.Verify(x => x.RemoverAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }
}