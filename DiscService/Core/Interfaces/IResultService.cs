using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

public interface IResultService
{
    Task<BotMessage> GetLastResultAsync(string chatId, Guid kafkaMessageId);
    Task<BotMessage> CompareResults(string chatId, Guid kafkaMessageId);
}