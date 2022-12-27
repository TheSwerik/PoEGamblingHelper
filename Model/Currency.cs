using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class Currency : CustomIdEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal ChaosEquivalent { get; set; }
    public string DetailsId { get; set; }
}