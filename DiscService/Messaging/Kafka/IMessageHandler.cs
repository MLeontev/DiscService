using DiscService.Messaging.Models;

namespace DiscService.Messaging.Kafka;

public interface IMessageHandler
{
    Task<BotMessage?> HandleAsync(BotMessage incoming);
}