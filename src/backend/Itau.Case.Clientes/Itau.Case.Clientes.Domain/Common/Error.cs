namespace Itau.Case.Clientes.Domain.Common;

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static Error NotFound(string message) => new("NotFound", message, ErrorType.NotFound);
    public static Error Validation(string message) => new("Validation", message, ErrorType.Validation);
    public static Error Conflict(string message) => new("Conflict", message, ErrorType.Conflict);
    public static Error Failure(string message) => new("Failure", message, ErrorType.Failure);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}
