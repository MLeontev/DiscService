using System.Text.Json;
using Confluent.Kafka;
using DiscService.Bot.Commands;
using DiscService.Bot.Messaging.Interfaces;
using DiscService.Bot.Messaging.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscService.Bot.Messaging.Kafka;

/// <summary>
/// Реализация регистратора сервиса, использующая Kafka для обмена сообщениями.
/// </summary>
public class ServiceRegistrar : IServiceRegistrar
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<ServiceRegistrar> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ServiceRegistrar"/>.
    /// </summary>
    /// <param name="kafkaSettings">Настройки подключения к Kafka.</param>
    /// <param name="producer">Продюсер Kafka для отправки сообщений.</param>
    /// <param name="logger">Логгер для записи информации о работе регистратора.</param>
    public ServiceRegistrar(
        IOptions<KafkaSettings> kafkaSettings,
        IProducer<Null, string> producer,
        ILogger<ServiceRegistrar> logger)
    {
        _producer = producer;
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
    }

    /// <inheritdoc/>
    public async Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = $"{_kafkaSettings.ServiceName}-registration-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(_kafkaSettings.InfoResponseTopic);

        var registrationRequest = new ServiceRegistrationRequest
        {
            Name = _kafkaSettings.ServiceName,
            Description = "Сервис для прохождения DISC-тестирования",
            Commands =
            [
                new CommandInfo(BotCommands.GetInfoCommand, "Получить описание психотипов по DISC", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.StartTestCommand, "Начать DISC-тестирование", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.LastResultCommand, "Получить результат последнего DISC-тестирования", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.CompareResultsCommand, "Сравнить результаты DISC-тестирования", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.CancelTestCommand, "Прервать DISC-тестирование", "ADD", "ANONYMOUS"),

                new CommandInfo(BotCommands.AnswerA, "Ответ A на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.AnswerB, "Ответ Б на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.AnswerC, "Ответ В на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.AnswerD, "Ответ Г на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),

                new CommandInfo(BotCommands.GetInfoCallback, "Получить описание психотипов по DISC", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.CompareResultsCallback, "Сравнить результаты DISC-тестирования", "ADD", "ANONYMOUS"),
                new CommandInfo(BotCommands.BeginTestCallback, "Начать DISC-тестирование", "ADD", "ANONYMOUS"),
            ]
        };

        var requestJson = JsonSerializer.Serialize(registrationRequest);

        await _producer.ProduceAsync(
            _kafkaSettings.InfoRequestTopic,
            new Message<Null, string> { Value = requestJson },
            stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);

                var response = JsonSerializer.Deserialize<ServiceRegistrationResponse>(consumeResult.Message.Value);

                if (response?.ServiceName == _kafkaSettings.ServiceName)
                {
                    return (response.ConsumeTopic, response.ProduceTopic);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при регистрации сервиса: {}", ex.Message);
                throw;
            }
        }

        throw new InvalidOperationException("Регистрация сервиса не завершена — не получен ответ от Kafka.");
    }
}