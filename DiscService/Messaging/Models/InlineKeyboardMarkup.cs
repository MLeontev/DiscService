using System.Text.Json.Serialization;

namespace DiscService.Messaging.Models;

public class InlineKeyboardMarkup
{
    [JsonPropertyName("inline_keyboard")]
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }

    public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inlineKeyboard)
    {
        InlineKeyboard = inlineKeyboard;
    }
}
