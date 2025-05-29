using DiscService.Bot.Messaging.Models;

namespace DiscService.Bot.Messaging.Interfaces;

/// <summary>
/// Интерфейс обработчика входящих сообщений бота.
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// Обрабатывает входящее сообщение бота и возвращает ответное сообщение.
    /// </summary>
    /// <param name="incoming">Входящее сообщение.</param>
    /// <returns>Ответное сообщение бота или null, если обработка невозможна.</returns>
    Task<BotMessage?> HandleAsync(BotMessage incoming);
}