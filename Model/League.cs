namespace Model;

public class League : Entity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public string Version { get; set; }

    public override string ToString()
    {
        return $"[id={Id}, Version={Version}, Name={Name}, StartDate={StartDate.ToLongDateString()}]";
    }
}