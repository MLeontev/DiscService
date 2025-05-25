using DiscService.Models;

namespace DiscService.Services.Utils;

public static class KeyboardBuilder
{
    private static readonly Dictionary<string, string> LabelMap = new Dictionary<string, string>
    {
        ["А"] = "A",
        ["Б"] = "B",
        ["В"] = "C",
        ["Г"] = "D"
    };

    public static object BuildAnswerKeyboard(Question question)
    {
        return new
        {
            inline_keyboard = question.Answers
                .Select(a => new
                {
                    text = a.Label,
                    callback_data = $"disc_answer_{LabelMap[a.Label]}"
                })
                .Chunk(2)
                .Select(chunk => chunk.ToArray())
                .ToArray()
        };
    }

    public static object BuildDiscInfoKeyboard()
    {
        return new
        {
            inline_keyboard = new[]
            {
                new[]
                {
                    new
                    {
                        text = "📚 Получить описание психотипов",
                        callback_data = "disc_info"
                    }
                }
            }
        };
    }
}