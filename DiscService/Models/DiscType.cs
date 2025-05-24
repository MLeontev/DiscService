namespace DiscService.Models;

public enum DiscType
{
    Dominance,
    Influence,
    Steadiness,
    Compliance
}

public static class DiscTypeExtensions
{
    public static string ToEmoji(this DiscType discType)
    {
        return discType switch
        {
            DiscType.Dominance => "🔴",
            DiscType.Influence => "🟡",
            DiscType.Steadiness => "🟢",
            DiscType.Compliance => "🔵",
            _ => "❓"
        };
    }
}