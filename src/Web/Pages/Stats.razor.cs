namespace Web.Pages;

public partial class Stats
{
    private readonly int[] _data = { Random.Shared.Next(10000), Random.Shared.Next(10000), Random.Shared.Next(10000) };
    private bool _isMyAccountSelected = true;

    private double LuckScore()
    {
        Console.WriteLine(Random.Shared.Next(10000));
        Console.WriteLine(_data[2]);
        return _data.Length > 0
                   ? _data[2] / (double)_data.Sum()
                   : 0;
    }

    private string LuckAdjective()
    {
        return LuckScore() switch
               {
                   > 0.9 => "a streamer",
                   > 0.8 => "amazing",
                   > 0.7 => "good",
                   > 0.55 => "decent",
                   > 0.45 => "average",
                   > 0.3 => "bad",
                   > 0.2 => "terrible",
                   _ => "abominable"
               };
    }
}