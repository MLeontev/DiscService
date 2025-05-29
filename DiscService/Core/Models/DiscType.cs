namespace DiscService.Core.Models;

/// <summary>
/// Перечисление психотипов по модели DISC.
/// </summary>
public enum DiscType
{
    Dominance,
    Influence,
    Steadiness,
    Compliance
}

/// <summary>
/// Расширения для перечисления <see cref="DiscType"/>.
/// </summary>
public static class DiscTypeExtensions
{
    /// <summary>
    /// Возвращает emoji, соответствующее типу DISC.
    /// </summary>
    /// <param name="discType">Тип DISC.</param>
    /// <returns>Строка с emoji-символом.</returns>
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