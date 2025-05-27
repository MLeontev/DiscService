using DiscService.Messaging.Models;

namespace DiscService.Messaging.Kafka.Interfaces;

public interface IMessageHandler
{
    Task<BotMessage?> HandleAsync(BotMessage incoming);
}