using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;
using Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;
using Itau.Case.Clientes.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Case.Clientes.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var query = new ObterTodosClientesQuery();
        var resultado = await mediator.Send(query);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var query = new ObterClientePorIdQuery(id);
        var resultado = await mediator.Send(query);
        
        if (!resultado.IsSuccess)
            return NotFound(resultado);
            
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteRequest request)
    {
        var command = new CriarClienteCommand(request.Nome, request.Email);
        var resultado = await mediator.Send(command);
        
        if (!resultado.IsSuccess)
        {
            return resultado.ErrorCode switch
            {
                "Validation" => BadRequest(resultado),
                "Conflict" => Conflict(resultado),
                _ => StatusCode(500, resultado)
            };
        }
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data!.Id }, resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteRequest request)
    {
        var command = new AtualizarClienteCommand(id, request.Nome, request.Email);
        var resultado = await mediator.Send(command);
        
        if (!resultado.IsSuccess)
        {
            return resultado.ErrorCode switch
            {
                "NotFound" => NotFound(resultado),
                "Validation" => BadRequest(resultado),
                "Conflict" => Conflict(resultado),
                _ => StatusCode(500, resultado)
            };
        }
        
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var command = new DeletarClienteCommand(id);
        var resultado = await mediator.Send(command);
        
        if (!resultado.IsSuccess)
        {
            return resultado.ErrorCode == "NotFound" 
                ? NotFound(resultado) 
                : StatusCode(500, resultado);
        }
        
        return NoContent();
    }

    [HttpPost("{id:int}/depositar")]
    public async Task<IActionResult> Depositar(int id, [FromBody] DepositarRequest request)
    {
        var command = new DepositarCommand(id, request.Valor, request.Descricao);
        var resultado = await mediator.Send(command);
        
        if (!resultado.IsSuccess)
        {
            return resultado.ErrorCode switch
            {
                "NotFound" => NotFound(resultado),
                "Validation" => BadRequest(resultado),
                _ => StatusCode(500, resultado)
            };
        }
        
        return Ok(resultado);
    }

    [HttpPost("{id:int}/sacar")]
    public async Task<IActionResult> Sacar(int id, [FromBody] SacarRequest request)
    {
        var command = new SacarCommand(id, request.Valor, request.Descricao);
        var resultado = await mediator.Send(command);
        
        if (!resultado.IsSuccess)
        {
            return resultado.ErrorCode switch
            {
                "NotFound" => NotFound(resultado),
                "Validation" => BadRequest(resultado),
                _ => StatusCode(500, resultado)
            };
        }
        
        return Ok(resultado);
    }
}