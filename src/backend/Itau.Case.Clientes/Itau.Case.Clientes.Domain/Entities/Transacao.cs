using Itau.Case.Clientes.Domain.Base;
using Itau.Case.Clientes.Domain.Enums;
using Itau.Case.Clientes.Domain.Exceptions;

namespace Itau.Case.Clientes.Domain.Entities;

public class Transacao : EntityBase
{
    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public ETipoTransacao Tipo { get; private set; }
    public decimal Valor { get; private set; }
    public string? Descricao { get; private set; }
    public DateTime DataTransacao { get; private set; }

    public Transacao(ETipoTransacao tipo, decimal valor, string? descricao = null)
    {  
        DomainException.When(valor <= 0, "O valor da transação deve ser maior que zero.");
   
        Tipo = tipo;
        Valor = valor;
        Descricao = descricao;
        DataTransacao = DateTime.UtcNow;
    }
}
