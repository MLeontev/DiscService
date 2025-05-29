using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Представляет кнопку inline-клавиатуры Telegram.
/// </summary>
public class InlineKeyboardButton
{
    /// <summary>
    /// Текст, отображаемый на кнопке.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// Значение callback, которое будет отправлено при нажатии на кнопку.
    /// </summary>
    [JsonPropertyName("callback_data")]
    public string CallbackData { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InlineKeyboardButton"/>.
    /// </summary>
    /// <param name="text">Текст кнопки.</param>
    /// <param name="callbackData">Callback-команда.</param>
    public InlineKeyboardButton(string text, string callbackData)
    {
        Text = text;
        CallbackData = callbackData;
    }
}
