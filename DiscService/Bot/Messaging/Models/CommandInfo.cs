using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Представляет информацию о команде бота.
/// Используется для регистрации доступных команд.
/// </summary>
public class CommandInfo
{
    /// <summary>
    /// Название команды.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Описание команды.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Действие, которое должно быть выполнено над командой.
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; }

    
    /// <summary>
    /// Права доступа, необходимые для использования команды.
    /// </summary>
    [JsonPropertyName("right")]
    public string Right { get; set; }
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CommandInfo"/>.
    /// </summary>
    /// <param name="name">Название команды.</param>
    /// <param name="description">Описание команды.</param>
    /// <param name="action">Действие, которое должно быть выполнено над командой.</param>
    /// <param name="right">Требуемые права доступа.</param>
    public CommandInfo(string name, string description, string action, string right)
    {
        Name = name;
        Description = description;
        Action = action;
        Right = right;
    }
}