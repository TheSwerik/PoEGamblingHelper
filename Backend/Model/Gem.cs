using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

public class Gem : CustomIdEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int MaxLevel()
    {
        return Name.Contains("Awakened") ? 5 :
               Name.Contains("Enhance") || Name.Contains("Empower") || Name.Contains("Enlighten") ? 3 :
               20;
    }

    public override string ToString() { return $"[Id={Id}, Name={Name}, MaxLevel={MaxLevel()}]"; }
}