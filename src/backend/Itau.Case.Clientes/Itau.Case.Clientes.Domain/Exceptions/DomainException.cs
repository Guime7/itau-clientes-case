namespace Itau.Case.Clientes.Domain.Exceptions;
public class DomainException(string message) : Exception(message)
{
    public static void When(bool hasError, string message)
    {
        if (hasError)
            throw new DomainException(message);
    }
}
