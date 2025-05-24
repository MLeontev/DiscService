using System.Text.Json.Serialization;

namespace DiscService.Messaging.Models;

public class BotMessage
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("data")]
    public SendMessageData Data { get; set; }

    [JsonPropertyName("kafkaMessageId")]
    public Guid KafkaMessageId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}