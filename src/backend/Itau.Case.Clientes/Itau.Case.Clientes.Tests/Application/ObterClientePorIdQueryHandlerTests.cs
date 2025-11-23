using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Entities;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Application;

public class ObterClientePorIdQueryHandlerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly ObterClientePorIdQueryHandler _handler;

    public ObterClientePorIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _handler = new ObterClientePorIdQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarCliente_QuandoExistir()
    {
        // Arrange
        var cliente = new Cliente("João Silva", "joao@email.com");
        var query = new ObterClientePorIdQuery(1);

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Nome.Should().Be("João Silva");
        result.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task Handle_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var query = new ObterClientePorIdQuery(999);

        _mockRepository
            .Setup(x => x.ObterPorIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
