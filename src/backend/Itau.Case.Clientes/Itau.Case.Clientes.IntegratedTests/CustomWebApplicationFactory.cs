using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Itau.Case.Clientes.Infrastructure.Data;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Infrastructure.Repositories;

namespace Itau.Case.Clientes.IntegratedTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover o DbContext MySQL existente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ClientesDbContext));

            if (descriptor != null)
                services.Remove(descriptor);

            // Adicionar um DbContext em memória para testes
            services.AddSingleton(new ClientesDbContext("Server=localhost;Port=3306;Database=case_clientes_test;User=case_user;Password=case_password;"));
            services.AddScoped<IClienteRepository, ClienteRepository>();
        });
    }
}
