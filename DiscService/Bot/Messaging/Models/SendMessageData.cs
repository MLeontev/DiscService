using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Представляет сообщение, передаваемое между ботом и сервисами через Kafka.
/// </summary>
public class MessageData
{
    /// <summary>
    /// Id чата Telegram, куда должно быть отправлено сообщение.
    /// </summary>
    [JsonPropertyName("chat_id")]
    public string? ChatId { get; set; }
    
    /// <summary>
    /// Текст сообщения.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Метод Telegram API.
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    /// <summary>
    /// Форматирование текста, поддерживаемое Telegram.
    /// </summary>
    [JsonPropertyName("parse_mode")]
    public string? ParseMode { get; set; }

    /// <summary>
    /// Инлайн-клавиатура, прилагаемая к сообщению.
    /// </summary>
    [JsonPropertyName("reply_markup")]
    public InlineKeyboardMarkup? ReplyMarkup { get; set; }
}
