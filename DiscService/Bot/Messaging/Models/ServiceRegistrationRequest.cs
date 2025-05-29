using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Запрос на регистрацию сервиса в Command Manager.
/// </summary>
public class ServiceRegistrationRequest
{
    /// <summary>
    /// Название сервиса.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Описание сервиса.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Список команд сервиса.
    /// </summary>
    [JsonPropertyName("commands")]
    public List<CommandInfo> Commands { get; set; }
}