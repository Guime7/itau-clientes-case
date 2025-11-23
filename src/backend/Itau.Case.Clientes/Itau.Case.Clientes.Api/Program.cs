using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Infrastructure.Data;
using Itau.Case.Clientes.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200") // URL do Angular em dev
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comentado para funcionar em Docker sem certificado HTTPS
// app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }