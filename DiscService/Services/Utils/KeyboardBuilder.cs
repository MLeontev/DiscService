using DiscService.Constants;
using DiscService.Messaging.Models;
using DiscService.Models;

namespace DiscService.Services.Utils;

public static class KeyboardBuilder
{
    private static readonly Dictionary<string, string> LabelMap = new()
    {
        ["А"] = "A",
        ["Б"] = "B",
        ["В"] = "C",
        ["Г"] = "D"
    };

    public static InlineKeyboardMarkup BuildAnswerKeyboard(Question question)
    {
        var rows = question.Answers
            .Select(a => new InlineKeyboardButton(
                $"{a.Label}",
                $"{BotCommands.AnswerPrefix}{LabelMap[a.Label]}"
            ))
            .Chunk(2)
            .Select(chunk => chunk.ToList())
            .ToList();

        return new InlineKeyboardMarkup(rows);
    }

    public static InlineKeyboardMarkup BuildDiscInfoKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("📚 Получить описание психотипов", BotCommands.GetInfoCallback)]
        ]);
    }

    public static InlineKeyboardMarkup BuildBeginTestKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("🚀 Начать тест", BotCommands.BeginTestCallback)]
        ]);
    }

    public static InlineKeyboardMarkup BuildTestResultKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("📚 Получить описание психотипов", BotCommands.GetInfoCallback)],
            [new InlineKeyboardButton("📊 Сравнить с предыдущим результатом", BotCommands.CompareResultsCallback)]
        ]);
    }
}