namespace Itau.Case.Clientes.Domain.Dtos;

public record ClienteDto(
    int Id,  
    string Nome, 
    string Email, 
    decimal Saldo, 
    DateTime DataCriacao, 
    DateTime DataAtualizacao);
