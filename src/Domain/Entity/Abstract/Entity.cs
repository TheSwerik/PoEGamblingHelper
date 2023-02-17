using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Abstract;

public abstract class Entity<T>
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public T Id { get; set; }

    public static Type KeyType() { return typeof(T); }
}