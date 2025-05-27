using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

public interface ITestService
{
    BotMessage StartTest(string chatId, Guid kafkaMessageId);
    BotMessage? BeginTest(string chatId, Guid kafkaMessageId);
    Task<BotMessage?> AnswerQuestion(string chatId, string callbackData, Guid kafkaMessageId);
    BotMessage CancelTest(string chatId, Guid kafkaMessageId);
}