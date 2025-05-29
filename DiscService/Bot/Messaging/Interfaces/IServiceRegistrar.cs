namespace DiscService.Bot.Messaging.Interfaces;

/// <summary>
/// Интерфейс для регистрации сервиса в Command Manager.
/// </summary>
public interface IServiceRegistrar
{
    /// <summary>
    /// Асинхронно регистрирует сервис и получает Kafka-топики для обмена сообщениями.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены операции.</param>
    /// <returns>
    /// Кортеж, содержащий:
    /// <list type="bullet">
    /// <item><description>consumeTopic: топик для получения сообщений</description></item>
    /// <item><description>produceTopic: топик для отправки сообщений</description></item>
    /// </list>
    /// </returns>
    Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken);
}