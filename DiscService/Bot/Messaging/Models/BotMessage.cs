using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

public class BotMessage
{
    /// <summary>
    /// Метод Telegram API.
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    /// <summary>
    /// Имя файла.
    /// </summary>
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    /// <summary>
    /// Данные сообщения.
    /// </summary>
    [JsonPropertyName("data")]
    public MessageData Data { get; set; }

    /// <summary>
    /// Id Kafka-сообщения.
    /// </summary>
    [JsonPropertyName("kafkaMessageId")]
    public Guid KafkaMessageId { get; set; }

    /// <summary>
    /// Статус сообщения.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Создает сообщение для отправки текста пользователю.
    /// </summary>
    /// <param name="chatId">ID чата получателя.</param>
    /// <param name="kafkaMessageId">ID Kafka-сообщения.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <param name="replyMarkup">Инлайн-клавиатура (по умолчанию null).</param>
    /// <param name="status">Статус (по умолчанию "COMPLETED").</param>
    /// <param name="parseMode">Форматирование текста (по умолчанию "Markdown").</param>
    /// <returns>Готовое для отправки сообщение.</returns>
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