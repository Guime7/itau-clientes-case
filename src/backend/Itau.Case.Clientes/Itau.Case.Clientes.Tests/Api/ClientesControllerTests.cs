using FluentAssertions;
using Itau.Case.Clientes.Api.Controllers;
using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;
using Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;
using Itau.Case.Clientes.Domain.Dtos;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Itau.Case.Clientes.UnitTests.Api;

public class ClientesControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new ClientesController(_mockMediator.Object);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarOk_ComListaDeClientes()
    {
        // Arrange
        var clientes = new List<ClienteDto>
        {
            new ClienteDto(1, "João", "joao@email.com", 0, DateTime.UtcNow, DateTime.UtcNow),
            new ClienteDto(2, "Maria", "maria@email.com", 0, DateTime.UtcNow, DateTime.UtcNow)
        };

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ObterTodosClientesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _controller.ObterTodos();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(clientes);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoClienteExistir()
    {
        // Arrange
        var cliente = new ClienteDto(1, "João", "joao@email.com", 0, DateTime.UtcNow, DateTime.UtcNow);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ObterClientePorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _controller.ObterPorId(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(cliente);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoClienteNaoExistir()
    {
        // Arrange
        _mockMediator
            .Setup(x => x.Send(It.IsAny<ObterClientePorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteDto?)null);

        // Act
        var result = await _controller.ObterPorId(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        // Arrange
        var request = new CriarClienteRequest("João", "joao@email.com");
        var cliente = new ClienteDto(1, "João", "joao@email.com", 0, DateTime.UtcNow, DateTime.UtcNow);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CriarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _controller.Criar(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoDomainException()
    {
        // Arrange
        var request = new CriarClienteRequest("", "joao@email.com");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CriarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DomainException("O nome não pode ser vazio."));

        // Act
        var result = await _controller.Criar(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Criar_DeveRetornarConflict_QuandoEmailJaExiste()
    {
        // Arrange
        var request = new CriarClienteRequest("João", "joao@email.com");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CriarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Email já existe"));

        // Act
        var result = await _controller.Criar(request);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task Atualizar_DeveRetornarOk_QuandoDadosValidos()
    {
        // Arrange
        var request = new AtualizarClienteRequest("João Santos", "joao.santos@email.com");
        var cliente = new ClienteDto(1, "João Santos", "joao.santos@email.com", 0, DateTime.UtcNow, DateTime.UtcNow);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<AtualizarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _controller.Atualizar(1, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Atualizar_DeveRetornarNotFound_QuandoClienteNaoExistir()
    {
        // Arrange
        var request = new AtualizarClienteRequest("João", "joao@email.com");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<AtualizarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Cliente não encontrado"));

        // Act
        var result = await _controller.Atualizar(999, request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Deletar_DeveRetornarNoContent_QuandoSucesso()
    {
        // Arrange
        _mockMediator
            .Setup(x => x.Send(It.IsAny<DeletarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Deletar(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Deletar_DeveRetornarNotFound_QuandoClienteNaoExistir()
    {
        // Arrange
        _mockMediator
            .Setup(x => x.Send(It.IsAny<DeletarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Cliente não encontrado"));

        // Act
        var result = await _controller.Deletar(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Depositar_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        var request = new DepositarRequest(100m, "Depósito");
        var resultado = new DepositarCommandResult(1, 0, 100m, 100m);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<DepositarCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        // Act
        var result = await _controller.Depositar(1, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Depositar_DeveRetornarBadRequest_QuandoValorInvalido()
    {
        // Arrange
        var request = new DepositarRequest(-100m, "Depósito");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<DepositarCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DomainException("Valor inválido"));

        // Act
        var result = await _controller.Depositar(1, request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Sacar_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        var request = new SacarRequest(50m, "Saque");
        var resultado = new SacarCommandResult(1, 100m, 50m, 50m);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<SacarCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        // Act
        var result = await _controller.Sacar(1, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Sacar_DeveRetornarBadRequest_QuandoSaldoInsuficiente()
    {
        // Arrange
        var request = new SacarRequest(1000m, "Saque");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<SacarCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DomainException("Saldo insuficiente"));

        // Act
        var result = await _controller.Sacar(1, request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
