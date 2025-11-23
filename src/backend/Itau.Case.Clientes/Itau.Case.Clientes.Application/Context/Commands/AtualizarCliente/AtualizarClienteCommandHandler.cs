using Itau.Case.Clientes.Application.Common.Mediator;
using Itau.Case.Clientes.Application.Interfaces;
using Itau.Case.Clientes.Domain.Common;
using Itau.Case.Clientes.Domain.Dtos;
using Itau.Case.Clientes.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Itau.Case.Clientes.Application.Context.Commands.AtualizarCliente;

public class AtualizarClienteHandler(IClienteRepository clienteRepository, ILogger<AtualizarClienteHandler> logger) 
    : IHandler<AtualizarClienteCommand, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Atualizando cliente. Id: {ClienteId}", request.Id);

            var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken);
            if (cliente == null)
            {
                logger.LogWarning("Cliente não encontrado. Id: {ClienteId}", request.Id);
                return Error.NotFound("Cliente não encontrado.");
            }

            var emailExiste = await clienteRepository.ExisteEmailAsync(request.Email, request.Id, cancellationToken);
            if (emailExiste)
            {
                logger.LogWarning("Email já cadastrado para outro cliente. Email: {Email}", request.Email);
                return Error.Conflict("Já existe outro cliente cadastrado com este email.");
            }

            cliente.AtualizarNome(request.Nome);
            cliente.AtualizarEmail(request.Email);

            await clienteRepository.AtualizarAsync(cliente, cancellationToken);

            logger.LogInformation("Cliente atualizado com sucesso. Id: {ClienteId}", cliente.Id);

            var dto = new ClienteDto(
                cliente.Id,
                cliente.Nome,
                cliente.Email,
                cliente.Saldo,
                cliente.DataCriacao,
                cliente.DataAtualizacao);

            return Result<ClienteDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Erro de validação ao atualizar cliente. Id: {ClienteId}", request.Id);
            return Error.Validation(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao atualizar cliente. Id: {ClienteId}", request.Id);
            return Error.Failure("Erro ao atualizar cliente.");
        }
    }
}
