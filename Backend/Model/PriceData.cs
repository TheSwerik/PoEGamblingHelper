namespace Backend.Model;

public class PriceData
{
    public GemData[] Lines { get; set; }
    public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
}