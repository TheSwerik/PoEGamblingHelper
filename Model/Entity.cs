using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public abstract class Entity : CustomIdEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
}