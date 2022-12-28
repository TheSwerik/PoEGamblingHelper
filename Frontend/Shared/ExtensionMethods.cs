namespace PoEGamblingHelper3.Shared;

public static class ExtensionMethods
{
    public static string Round(this decimal value, int places) { return string.Format($"{{0:N{places}}}", value); }
}