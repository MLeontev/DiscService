using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

public interface IDiscInfoService
{
    BotMessage GetDiscInfo(string chatId, Guid kafkaMessageId);
}