using DiscService.Bot.Messaging.Models;

namespace DiscService.Bot.Messaging.Interfaces;

public interface IMessageHandler
{
    Task<BotMessage?> HandleAsync(BotMessage incoming);
}