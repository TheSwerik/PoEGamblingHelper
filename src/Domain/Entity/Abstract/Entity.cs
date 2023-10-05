namespace PoEGamblingHelper.Domain.Entity.Abstract;

public abstract class Entity<T>
{
    public T Id { get; set; } = default!;
}