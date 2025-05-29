using DiscService.Bot.Messaging.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления прохождением DISC-теста.
/// </summary>
public interface ITestService
{
    /// <summary>
    /// Формирует сообщение с приглашением начать тест.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя..</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение для начала теста.</returns>
    BotMessage StartTest(string chatId, Guid kafkaMessageId);
    
    /// <summary>
    /// Создает сессию теста и возвращает первый вопрос.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя..</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение с первым вопросом или null, если не удалось начать тест.</returns>
    BotMessage? BeginTest(string chatId, Guid kafkaMessageId);
    
    /// <summary>
    /// Обрабатывает ответ пользователя на текущий вопрос.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя..</param>
    /// <param name="callbackData">Callback-команда из ответа пользователя.</param>
    /// <param name="kafkaMessageId">Id Kafka-сообщения.</param>
    /// <returns>Сообщение со следующим вопросом или результатом, либо null.</returns>
    Task<BotMessage?> AnswerQuestion(string chatId, string callbackData, Guid kafkaMessageId);
    
    /// <summary>
    /// Отменяет текущую сессию теста пользователя.
    /// </summary>
    /// <param name="chatId">Id Telegram-чата пользователя..</param>
    /// <param name="kafkaMessageId">Идентификатор сообщения Kafka.</param>
    /// <returns>Сообщение об отмене теста или null.</returns>
    BotMessage? CancelTest(string chatId, Guid kafkaMessageId);
}