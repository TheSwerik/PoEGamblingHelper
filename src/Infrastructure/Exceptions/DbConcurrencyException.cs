using PoEGamblingHelper.Application.Exception.Abstract;

namespace PoEGamblingHelper.Infrastructure.Exceptions;

public class DbConcurrencyException : PoeGamblingHelperException
{
    public DbConcurrencyException(string? message) : base(message) { }
}