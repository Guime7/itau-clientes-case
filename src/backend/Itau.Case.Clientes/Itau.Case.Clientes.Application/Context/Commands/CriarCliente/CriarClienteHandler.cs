using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;
using Itau.Case.Clientes.Domain.Entities;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Commands.CriarCliente;

public class CriarClienteHandler(IClienteRepository clienteRepository, ILogger<CriarClienteHandler> logger) 
    : IHandler<CriarClienteCommand, Result<ClienteDto>>
{
     public async Task<Result<ClienteDto>> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Iniciando criação de cliente. Email: {Email}", request.Email);

            var emailExiste = await clienteRepository.ExisteEmailAsync(request.Email, null, cancellationToken);
            if (emailExiste)
            {
                logger.LogWarning("Tentativa de criar cliente com email duplicado: {Email}", request.Email);
                return Error.Conflict("Já existe um cliente cadastrado com este email.");
            }

            var novoCliente = new Cliente(request.Nome, request.Email);
            
            var clienteCriado = await clienteRepository.AdicionarAsync(novoCliente, cancellationToken);

            logger.LogInformation("Cliente criado com sucesso. Id: {ClienteId}, Email: {Email}", clienteCriado.Id, clienteCriado.Email);

            var dto = new ClienteDto(
                clienteCriado.Id, 
                clienteCriado.Nome, 
                clienteCriado.Email, 
                clienteCriado.Saldo, 
                clienteCriado.DataCriacao, 
                clienteCriado.DataAtualizacao);

            return Result<ClienteDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Erro de validação ao criar cliente. Email: {Email}", request.Email);
            return Error.Validation(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao criar cliente. Email: {Email}", request.Email);
            return Error.Failure("Erro ao criar cliente.");
        }
    }
}
