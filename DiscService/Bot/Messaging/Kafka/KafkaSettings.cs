namespace DiscService.Bot.Messaging.Kafka;

/// <summary>
/// Настройки подключения и конфигурации Kafka для сервиса.
/// </summary>
public class KafkaSettings
{
    /// <summary>
    /// Имя сервиса для регистрации.
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Адрес Kafka-брокера для подключения.
    /// </summary>
    public string BootstrapServers { get; set; } = string.Empty;

    /// <summary>
    /// Название Kafka-топика, в который отправляется запрос на регистрацию.
    /// </summary>
    public string InfoRequestTopic { get; set; } = string.Empty;

    /// <summary>
    /// Название Kafka-топика, из которого ожидается ответ на регистрацию.
    /// </summary>
    public string InfoResponseTopic { get; set; } = string.Empty;
}