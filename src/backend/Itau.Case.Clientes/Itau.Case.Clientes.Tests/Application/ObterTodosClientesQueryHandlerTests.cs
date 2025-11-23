using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class ObterTodosClientesQueryHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly ObterTodosClientesQueryHandler _handler;

    public ObterTodosClientesQueryHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new ObterTodosClientesQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDeClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new Cliente("João Silva", "joao@email.com"),
            new Cliente("Maria Santos", "maria@email.com")
        };

        var query = new ObterTodosClientesQuery();

        _mockRepository
            .Setup(x => x.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().Nome.Should().Be("João Silva");
        result.Last().Nome.Should().Be("Maria Santos");
    }

    [Fact]
    public async Task Handle_DeveRetornarListaVazia_QuandoNaoExistemClientes()
    {
        // Arrange
        var query = new ObterTodosClientesQuery();

        _mockRepository
            .Setup(x => x.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Cliente>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
