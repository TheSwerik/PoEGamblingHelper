using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public abstract class Entity<T> : IEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public T Id { get; set; }

    public static Type KeyType() { return typeof(T); }
}