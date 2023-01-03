namespace Model;

public class Currency : Entity<string>
{
    public string Name { get; set; } = string.Empty;
    public decimal ChaosEquivalent { get; set; }
    public string? Icon { get; set; }
}