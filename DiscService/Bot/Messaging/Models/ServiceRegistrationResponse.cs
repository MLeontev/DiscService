using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Ответ на запрос регистрации сервиса.
/// </summary>
public class ServiceRegistrationResponse
{
    /// <summary>
    /// Название зарегистрированного сервиса.
    /// </summary>
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; set; }

    /// <summary>
    /// Название Kafka-топика, который должен слушать сервис.
    /// </summary>
    [JsonPropertyName("consumeTopic")]
    public string ConsumeTopic { get; set; }

    /// <summary>
    /// Название Kafka-топика, в который сервис будет отправлять сообщения.
    /// </summary>
    [JsonPropertyName("produceTopic")]
    public string ProduceTopic { get; set; }

    /// <summary>
    /// Дополнительное сообщение
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}