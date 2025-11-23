using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Itau.Case.Clientes.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Tentativa de login - TipoUsuario: {TipoUsuario}, Email: {Email}", 
            request.TipoUsuario, request.Email);

        // Validação básica
        if (string.IsNullOrWhiteSpace(request.TipoUsuario) || 
            (request.TipoUsuario != "Admin" && request.TipoUsuario != "Cliente"))
        {
            return BadRequest(new { message = "TipoUsuario deve ser 'Admin' ou 'Cliente'" });
        }

        if (request.TipoUsuario == "Cliente" && !request.ClienteId.HasValue)
        {
            return BadRequest(new { message = "ClienteId é obrigatório para tipo Cliente" });
        }

        // Gerar token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = _configuration["Jwt:SecretKey"] ?? "UmaChaveMuitoSecretaParaOSeuCaseItau123!";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, request.TipoUsuario)
        };

        // Se for Cliente, adicionar o ID como NameIdentifier
        if (request.TipoUsuario == "Cliente" && request.ClienteId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, request.ClienteId.Value.ToString()));
            claims.Add(new Claim("ClienteId", request.ClienteId.Value.ToString()));
        }

        // Adicionar email se fornecido
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, request.Email));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        _logger.LogInformation("Token gerado com sucesso para {TipoUsuario}", request.TipoUsuario);

        return Ok(new LoginResponse
        {
            Token = tokenString,
            TipoUsuario = request.TipoUsuario,
            ClienteId = request.ClienteId,
            Email = request.Email,
            ExpiresAt = tokenDescriptor.Expires.Value
        });
    }
}

public record LoginRequest(
    string TipoUsuario, 
    int? ClienteId = null,
    string? Email = null,
    string? Senha = null // Aceito mas ignorado nesta implementação simplificada
);

public record LoginResponse
{
    public required string Token { get; init; }
    public required string TipoUsuario { get; init; }
    public int? ClienteId { get; init; }
    public string? Email { get; init; }
    public DateTime ExpiresAt { get; init; }
}
