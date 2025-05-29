using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс сервиса для получения информации о типах DISC.
/// </summary>
public interface IDiscInfoService
{
    /// <summary>
    /// Возвращает текстовое сообщение с информацией о всех типах DISC.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя..</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение с информацией о всех типах DISC</returns>
    BotMessage GetDiscInfo(string chatId, Guid kafkaMessageId);
}