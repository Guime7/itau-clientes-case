using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Domain.Dtos;
using Itau.Case.Clientes.IntegratedTests;

namespace Itau.Case.Clientes.IntegratedTests.ClientesController;

public class ClientesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ClientesControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarOk()
    {
        // Act
        var response = await _client.GetAsync("/api/clientes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var clientes = await response.Content.ReadFromJsonAsync<List<ClienteDto>>();
        clientes.Should().NotBeNull();
    }

    [Fact]
    public async Task Criar_DeveRetornarCreated_QuandoDadosValidos()
    {
        // Arrange
        var request = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var cliente = await response.Content.ReadFromJsonAsync<ClienteDto>();
        cliente.Should().NotBeNull();
        cliente!.Nome.Should().Be(request.Nome);
        cliente.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoNomeVazio()
    {
        // Arrange
        var request = new CriarClienteRequest("", "test@email.com");

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoClienteExiste()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        // Act
        var response = await _client.GetAsync($"/api/clientes/{cliente!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var clienteObtido = await response.Content.ReadFromJsonAsync<ClienteDto>();
        clienteObtido.Should().NotBeNull();
        clienteObtido!.Id.Should().Be(cliente.Id);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoClienteNaoExiste()
    {
        // Act
        var response = await _client.GetAsync("/api/clientes/999999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarOk_QuandoDadosValidos()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        var updateRequest = new AtualizarClienteRequest("Updated Name", $"updated{Guid.NewGuid()}@email.com");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/clientes/{cliente!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var clienteAtualizado = await response.Content.ReadFromJsonAsync<ClienteDto>();
        clienteAtualizado!.Nome.Should().Be("Updated Name");
    }

    [Fact]
    public async Task Atualizar_DeveRetornarNotFound_QuandoClienteNaoExiste()
    {
        // Arrange
        var updateRequest = new AtualizarClienteRequest("Updated Name", "updated@email.com");

        // Act
        var response = await _client.PutAsJsonAsync("/api/clientes/999999", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Deletar_DeveRetornarNoContent_QuandoClienteExiste()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/clientes/{cliente!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deletar_DeveRetornarNotFound_QuandoClienteNaoExiste()
    {
        // Act
        var response = await _client.DeleteAsync("/api/clientes/999999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Depositar_DeveRetornarOk_QuandoDadosValidos()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        var depositRequest = new DepositarRequest(100m, "Depósito teste");

        // Act
        var response = await _client.PostAsJsonAsync($"/api/clientes/{cliente!.Id}/depositar", depositRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultado = await response.Content.ReadFromJsonAsync<DepositarCommandResult>();
        resultado!.SaldoAtual.Should().Be(100m);
    }

    [Fact]
    public async Task Depositar_DeveRetornarNotFound_QuandoClienteNaoExiste()
    {
        // Arrange
        var depositRequest = new DepositarRequest(100m, "Depósito teste");

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes/999999/depositar", depositRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Sacar_DeveRetornarOk_QuandoDadosValidos()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        var depositRequest = new DepositarRequest(100m, "Depósito inicial");
        await _client.PostAsJsonAsync($"/api/clientes/{cliente!.Id}/depositar", depositRequest);

        var sacarRequest = new SacarRequest(50m, "Saque teste");

        // Act
        var response = await _client.PostAsJsonAsync($"/api/clientes/{cliente.Id}/sacar", sacarRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultado = await response.Content.ReadFromJsonAsync<SacarCommandResult>();
        resultado!.SaldoAtual.Should().Be(50m);
    }

    [Fact]
    public async Task Sacar_DeveRetornarBadRequest_QuandoSaldoInsuficiente()
    {
        // Arrange
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        var sacarRequest = new SacarRequest(100m, "Saque teste");

        // Act
        var response = await _client.PostAsJsonAsync($"/api/clientes/{cliente!.Id}/sacar", sacarRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sacar_DeveRetornarNotFound_QuandoClienteNaoExiste()
    {
        // Arrange
        var sacarRequest = new SacarRequest(50m, "Saque teste");

        // Act
        var response = await _client.PostAsJsonAsync("/api/clientes/999999/sacar", sacarRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task FluxoCompleto_DeveFuncionar()
    {
        // 1. Criar cliente
        var createRequest = new CriarClienteRequest($"Test User {Guid.NewGuid()}", $"test{Guid.NewGuid()}@email.com");
        var createResponse = await _client.PostAsJsonAsync("/api/clientes", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var cliente = await createResponse.Content.ReadFromJsonAsync<ClienteDto>();

        // 2. Obter cliente por ID
        var getResponse = await _client.GetAsync($"/api/clientes/{cliente!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 3. Depositar
        var depositRequest = new DepositarRequest(200m, "Depósito inicial");
        var depositResponse = await _client.PostAsJsonAsync($"/api/clientes/{cliente.Id}/depositar", depositRequest);
        depositResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 4. Sacar
        var sacarRequest = new SacarRequest(50m, "Saque");
        var sacarResponse = await _client.PostAsJsonAsync($"/api/clientes/{cliente.Id}/sacar", sacarRequest);
        sacarResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 5. Atualizar
        var updateRequest = new AtualizarClienteRequest("Updated Name", $"updated{Guid.NewGuid()}@email.com");
        var updateResponse = await _client.PutAsJsonAsync($"/api/clientes/{cliente.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 6. Verificar saldo final
        var finalGetResponse = await _client.GetAsync($"/api/clientes/{cliente.Id}");
        var finalCliente = await finalGetResponse.Content.ReadFromJsonAsync<ClienteDto>();
        finalCliente!.Saldo.Should().Be(150m);

        // 7. Deletar
        var deleteResponse = await _client.DeleteAsync($"/api/clientes/{cliente.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
