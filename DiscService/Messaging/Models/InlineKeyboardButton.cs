using System.Text.Json.Serialization;

namespace DiscService.Messaging.Models;

public class InlineKeyboardButton
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("callback_data")]
    public string CallbackData { get; set; }

    public InlineKeyboardButton(string text, string callbackData)
    {
        Text = text;
        CallbackData = callbackData;
    }
}
