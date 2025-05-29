using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с результатами DISC-теста.
/// </summary>
public interface IResultService
{
    /// <summary>
    /// Получает последний результат теста пользователя.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя.</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение с последним результатом теста.</returns>
    Task<BotMessage> GetLastResultAsync(string chatId, Guid kafkaMessageId);
    
    /// <summary>
    /// Сравнивает два последних результата DISC-теста пользователя.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя.</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение со сравнением результатов.</returns>
    Task<BotMessage> CompareResults(string chatId, Guid kafkaMessageId);
}