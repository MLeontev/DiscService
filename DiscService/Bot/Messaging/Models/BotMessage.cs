using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

public class BotMessage
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("data")]
    public MessageData Data { get; set; }

    [JsonPropertyName("kafkaMessageId")]
    public Guid KafkaMessageId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    public static BotMessage Create(
        string chatId,
        Guid kafkaMessageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null,
        string status = "COMPLETED",
        string? parseMode = "Markdown")
    {
        return new BotMessage
        {
            Method = "sendmessage",
            KafkaMessageId = kafkaMessageId,
            Status = status,
            Data = new MessageData
            {
                ChatId = chatId,
                Method = "sendmessage",
                Text = text,
                ParseMode = parseMode,
                ReplyMarkup = replyMarkup
            }
        };
    }
}