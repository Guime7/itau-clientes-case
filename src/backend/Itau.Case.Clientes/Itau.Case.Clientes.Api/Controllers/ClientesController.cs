using System.Security.Claims;
using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;
using Itau.Case.Clientes.Application.Context.Commands.CriarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DeletarCliente;
using Itau.Case.Clientes.Application.Context.Commands.DepositarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Commands.SacarSaldoCliente;
using Itau.Case.Clientes.Application.Context.Queries.ObterClientePorId;
using Itau.Case.Clientes.Application.Context.Queries.ObterTodosClientes;
using Itau.Case.Clientes.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itau.Case.Clientes.Api.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize] // Bloqueia tudo por padrão - requer autenticação
public class ClientesController(IMediator mediator) : ControllerBase
{
    // Somente Admin pode ver todos os clientes
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObterTodos()
    {
        var query = new ObterTodosClientesQuery();
        var resultado = await mediator.Send(query);
        return Ok(resultado);
    }

    // Admin ou o próprio Cliente podem ver os detalhes
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        // Validação de autorização: Admin pode ver tudo, Cliente só pode ver seus próprios dados
        if (!UsuarioPodeAcessar(id))
            return Forbid();

        var query = new ObterClientePorIdQuery(id);
        var resultado = await mediator.Send(query);
        
        if (!resultado.IsSuccess)
            return NotFound(resultado);
            
        return Ok(resultado);
    }

    // Somente Admin pode criar novos clientes
    [HttpPost]
    [Authorize(Roles = "Admin")]
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

    // Somente Admin pode atualizar clientes
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
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

    // Somente Admin pode deletar clientes
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
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

    // Admin pode depositar em qualquer conta, Cliente só na própria
    [HttpPost("{id:int}/depositar")]
    public async Task<IActionResult> Depositar(int id, [FromBody] DepositarRequest request)
    {
        // Validação de segurança: eu posso acessar essa conta?
        if (!UsuarioPodeAcessar(id))
            return Forbid();

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

    // Admin pode sacar de qualquer conta, Cliente só da própria
    [HttpPost("{id:int}/sacar")]
    public async Task<IActionResult> Sacar(int id, [FromBody] SacarRequest request)
    {
        // Validação de segurança: eu posso acessar essa conta?
        if (!UsuarioPodeAcessar(id))
            return Forbid();

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

    /// <summary>
    /// Verifica se o usuário atual pode acessar o recurso (cliente) especificado.
    /// Admin pode acessar qualquer recurso.
    /// Cliente só pode acessar seu próprio registro.
    /// </summary>
    private bool UsuarioPodeAcessar(int idRecurso)
    {
        var user = HttpContext.User;
        
        // Se for Admin, pode tudo
        if (user.IsInRole("Admin"))
            return true;

        // Se for Cliente, o ID do Token (NameIdentifier) deve ser igual ao ID da rota
        var idNoToken = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return idNoToken != null && idNoToken == idRecurso.ToString();
    }
}