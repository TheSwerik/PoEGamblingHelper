using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

public class Gem : CustomIdEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int MaxLevel()
    {
        var isAwakened = Name.Contains("Awakened");
        var isExceptional = Name.Contains("Enhance") || Name.Contains("Empower") || Name.Contains("Enlighten");
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    public override string ToString() { return $"[Id={Id}, Name={Name}, MaxLevel={MaxLevel()}]"; }
}