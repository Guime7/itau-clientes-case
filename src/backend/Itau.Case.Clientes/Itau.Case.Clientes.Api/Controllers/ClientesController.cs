using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;
using Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Case.Clientes.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        try
        {
            var query = new ObterTodosClientesQuery();
            var resultado = await mediator.Send(query);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao obter clientes.", erro = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var query = new ObterClientePorIdQuery(id);
            var resultado = await mediator.Send(query);

            if (resultado == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao obter cliente.", erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteRequest request)
    {
        try
        {
            var command = new CriarClienteCommand(request.Nome, request.Email);
            var resultado = await mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao criar cliente.", erro = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteRequest request)
    {
        try
        {
            var command = new AtualizarClienteCommand(id, request.Nome, request.Email);
            var resultado = await mediator.Send(command);
            return Ok(resultado);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return ex.Message.Contains("não encontrado") 
                ? NotFound(new { mensagem = ex.Message }) 
                : Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao atualizar cliente.", erro = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        try
        {
            var command = new DeletarClienteCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao deletar cliente.", erro = ex.Message });
        }
    }

    [HttpPost("{id:int}/depositar")]
    public async Task<IActionResult> Depositar(int id, [FromBody] DepositarRequest request)
    {
        try
        {
            var command = new DepositarCommand(id, request.Valor, request.Descricao);
            var resultado = await mediator.Send(command);
            return Ok(resultado);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao realizar depósito.", erro = ex.Message });
        }
    }

    [HttpPost("{id:int}/sacar")]
    public async Task<IActionResult> Sacar(int id, [FromBody] SacarRequest request)
    {
        try
        {
            var command = new SacarCommand(id, request.Valor, request.Descricao);
            var resultado = await mediator.Send(command);
            return Ok(resultado);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao realizar saque.", erro = ex.Message });
        }
    }
}