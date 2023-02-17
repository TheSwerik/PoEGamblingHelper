namespace Domain.Exception.Body;

public record PoeGamblingHelperExceptionBody(ExceptionType Type,
                                             ExceptionId Id,
                                             string? Message = null,
                                             object? Body = null)
{
    public ExceptionType Type { get; } = Type;
    public ExceptionId Id { get; } = Id;
    public string? Message { get; } = Message;
    public object? Body { get; } = Body;
}