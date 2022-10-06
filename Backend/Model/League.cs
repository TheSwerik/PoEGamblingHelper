namespace Backend.Model;

public class League : Entity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public override string ToString() { return $"[id={Id}, Name={Name}, StartDate={StartDate.ToLongDateString()}]"; }
}