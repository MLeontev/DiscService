using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

public class ServiceRegistrationResponse
{
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; set; }

    [JsonPropertyName("consumeTopic")]
    public string ConsumeTopic { get; set; }

    [JsonPropertyName("produceTopic")]
    public string ProduceTopic { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}