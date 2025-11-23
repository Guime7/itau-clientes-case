using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Infrastructure.Data;
using Itau.Case.Clientes.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithProperty("Application", "Itau.Case.Clientes.Api")
    .WriteTo.Console()
    .WriteTo.DatadogLogs(
        apiKey: Environment.GetEnvironmentVariable("DD_API_KEY") ?? "dummy-key",
        source: "csharp",
        service: "itau-case-clientes-api",
        host: Environment.MachineName,
        configuration: new Serilog.Sinks.Datadog.Logs.DatadogConfiguration { Url = "https://http-intake.logs.datadoghq.com" })
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Configurar JWT Authentication
var jwtKey = builder.Configuration["Jwt:SecretKey"] ?? "UmaChaveMuitoSecretaParaOSeuCaseItau123!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configurar conexão MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Port=3306;Database=case_clientes;User=case_user;Password=case_password;";
builder.Services.AddSingleton(new ClientesDbContext(connectionString));

// Registrar Repository
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Registrar Mediator customizado
builder.Services.AddScoped<IMediator, Mediator>();

// Registrar todos os Handlers automaticamente
var applicationAssembly = Assembly.Load("Itau.Case.Clientes.Application");
var handlerTypes = applicationAssembly.GetTypes()
    .Where(t => t.GetInterfaces().Any(i => 
        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>)));

foreach (var handlerType in handlerTypes)
{
    var handlerInterface = handlerType.GetInterfaces()
        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>));
    builder.Services.AddScoped(handlerInterface, handlerType);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:3000",
                "http://frontend:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comentado para funcionar em Docker sem certificado HTTPS
// app.UseHttpsRedirection();

// Configurar CORS baseado no ambiente
var corsPolicy = app.Environment.IsDevelopment() ? "AllowAll" : "AllowFrontend";
app.UseCors(corsPolicy);

app.UseSerilogRequestLogging();

// ORDEM IMPORTA: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando aplicação Itau.Case.Clientes.Api");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}

[ExcludeFromCodeCoverage]
public partial class Program { }