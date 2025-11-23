namespace Itau.Case.Clientes.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }
    public string? ErrorDescription { get; }

    private Result(bool isSuccess, T? data, string? message, string? errorCode, string? errorDescription)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        ErrorCode = errorCode;
        ErrorDescription = errorDescription;
    }

    public static Result<T> Success(T data, string? message = null) 
        => new(true, data, message ?? "Operação realizada com sucesso.", null, null);

    public static Result<T> Failure(string errorCode, string errorDescription) 
        => new(false, default, null, errorCode, errorDescription);

    public static Result<T> Failure(Error error) 
        => new(false, default, null, error.Code, error.Message);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }
    public string? ErrorDescription { get; }

    private Result(bool isSuccess, string? message, string? errorCode, string? errorDescription)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorCode = errorCode;
        ErrorDescription = errorDescription;
    }

    public static Result Success(string? message = null) 
        => new(true, message ?? "Operação realizada com sucesso.", null, null);

    public static Result Failure(string errorCode, string errorDescription) 
        => new(false, null, errorCode, errorDescription);

    public static Result Failure(Error error) 
        => new(false, null, error.Code, error.Message);

    public static implicit operator Result(Error error) => Failure(error);
}
