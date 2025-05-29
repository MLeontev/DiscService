using DiscService.Bot.Commands;
using DiscService.Bot.Messaging.Models;
using DiscService.Core.Models;

namespace DiscService.Bot.UI;

/// <summary>
/// Содержит методы для построения inline-клавиатур Telegram-бота.
/// </summary>
public static class KeyboardBuilder
{
    private static readonly Dictionary<string, string> LabelMap = new()
    {
        ["А"] = "A",
        ["Б"] = "B",
        ["В"] = "C",
        ["Г"] = "D"
    };

    /// <summary>
    /// Формирует inline-клавиатуру с вариантами ответов на вопрос DISC-теста.
    /// </summary>
    /// <param name="question">Вопрос с вариантами ответов.</param>
    /// <returns>Разметка inline-клавиатуры с кнопками ответов.</returns>
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
    /// <summary>
    /// Формирует inline-клавиатуру для получения информации о психотипах DISC.
    /// </summary>
    /// <returns>Разметка inline-клавиатуры с кнопкой запроса информации.</returns>
    public static InlineKeyboardMarkup BuildDiscInfoKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("📚 Получить описание психотипов", BotCommands.GetInfoCallback)]
        ]);
    }

    /// <summary>
    /// Формирует inline-клавиатуру для начала DISC-теста.
    /// </summary>
    /// <returns>Разметка inline-клавиатуры с кнопкой начала тестирования.</returns>
    public static InlineKeyboardMarkup BuildBeginTestKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("🚀 Начать тест", BotCommands.BeginTestCallback)]
        ]);
    }

    /// <summary>
    /// Формирует inline-клавиатуру для меню после прохождения теста.
    /// </summary>
    /// <returns>Разметка inline-клавиатуры с кнопками получения информации и сравнения результатов.</returns>
    public static InlineKeyboardMarkup BuildResultMenuKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [new InlineKeyboardButton("📚 Получить описание психотипов", BotCommands.GetInfoCallback)],
            [new InlineKeyboardButton("📊 Сравнить с предыдущим результатом", BotCommands.CompareResultsCallback)]
        ]);
    }
}