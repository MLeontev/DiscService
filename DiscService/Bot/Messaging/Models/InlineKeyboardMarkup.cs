using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

/// <summary>
/// Представляет inline-клавиатуру Telegram.
/// </summary>
public class InlineKeyboardMarkup
{
    /// <summary>
    /// Список строк кнопок, где каждая строка — список кнопок <see cref="InlineKeyboardButton"/>.
    /// </summary>
    [JsonPropertyName("inline_keyboard")]
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InlineKeyboardMarkup"/>.
    /// </summary>
    /// <param name="inlineKeyboard">Структура клавиатуры (список строк кнопок).</param>
    public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inlineKeyboard)
    {
        InlineKeyboard = inlineKeyboard;
    }
}
