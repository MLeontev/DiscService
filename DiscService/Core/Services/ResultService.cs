using DiscService.Bot.Commands;
using DiscService.Bot.Messaging.Models;
using DiscService.Bot.UI;
using DiscService.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscService.Core.Services;

public class ResultService
{
    private readonly AppDbContext _dbContext;

    public ResultService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BotMessage> GetLastResultAsync(string chatId, Guid kafkaMessageId)
    {
        var lastResult = await _dbContext.TestResults
            .Where(r => r.ChatId == chatId)
            .OrderByDescending(r => r.FinishedAt)
            .FirstOrDefaultAsync();

        if (lastResult == null)
        {
            return BotMessage.Create(chatId, kafkaMessageId, "Нет результатов тестов", parseMode: null);
        }

        return BotMessage.Create(
            chatId,
            kafkaMessageId,
            $"Последний результат (от {lastResult.FinishedAt:dd.MM.yyyy}):\n{MessageFormatter.FormatResult(lastResult)}",
            KeyboardBuilder.BuildResultMenuKeyboard());
    }

    public async Task<BotMessage> CompareResults(string chatId, Guid kafkaMessageId)
    {
        var results = await _dbContext.TestResults
        .Where(r => r.ChatId == chatId)
        .OrderByDescending(r => r.FinishedAt)
        .Take(2)
        .ToListAsync();

        if (results.Count < 2)
        {
            return BotMessage.Create(
                chatId,
                kafkaMessageId,
                $"Недостаточно данных. Пройдите тест {BotCommands.StartTestCommand} не менее 2-х раз для сравнения",
                parseMode: null);
        }

        var currentResult = results[0];
        var previousResult = results[1];

        var comparisonText = MessageFormatter.FormatComparison(previousResult, currentResult);

        return BotMessage.Create(chatId, kafkaMessageId, comparisonText, KeyboardBuilder.BuildDiscInfoKeyboard());
    }
}