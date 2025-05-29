using System.Text.Json;
using Confluent.Kafka;
using DiscService.Bot.Messaging.Interfaces;
using DiscService.Bot.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscService.Bot.Messaging.Kafka;

/// <summary>
/// Background-сервис для обработки сообщений от бота через Kafka и отправки ответов.
/// Используется для обмена командами и сообщениями между DISC-сервисом и ботом.
/// </summary>
public class KafkaCommandService : BackgroundService
{
    private readonly ILogger<KafkaCommandService> _logger;
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IServiceRegistrar _serviceRegistrar;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IConsumer<Ignore, string> _consumer;
    private string _consumeTopic;
    private string _produceTopic;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="KafkaCommandService"/>.
    /// </summary>
    /// <param name="logger">Логгер для записи информации о работе сервиса.</param>
    /// <param name="producer">Kafka producer для отправки сообщений.</param>
    /// <param name="kafkaSettings">Настройки подключения к Kafka.</param>
    /// <param name="serviceRegistrar">Сервис для регистрации и получения топиков.</param>
    /// <param name="serviceScopeFactory">Фабрика для создания скоупов зависимостей.</param>
    public KafkaCommandService(
        ILogger<KafkaCommandService> logger,
        IProducer<Null, string> producer,
        IOptions<KafkaSettings> kafkaSettings,
        IServiceRegistrar serviceRegistrar,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _producer = producer;
        _kafkaSettings = kafkaSettings.Value;
        _serviceRegistrar = serviceRegistrar;
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Запускает основной цикл обработки входящих сообщений из Kafka и отправки ответов.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены операции.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        (_consumeTopic, _produceTopic) = await _serviceRegistrar.RegisterAsync(stoppingToken);

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = $"{_kafkaSettings.ServiceName}-group",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _consumer.Subscribe(_consumeTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult == null) continue;

                var incomingMessage = JsonSerializer.Deserialize<BotMessage>(consumeResult.Message.Value);
                if (incomingMessage == null) continue;

                using var scope = _serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();

                var response = await handler.HandleAsync(incomingMessage);

                if (incomingMessage.Data.ChatId != null)
                    response ??= BotMessage.Create(
                        incomingMessage.Data.ChatId,
                        Guid.NewGuid(),
                        "Что-то пошло не так. Попробуйте ещё раз");

                var json = JsonSerializer.Serialize(response);
                await _producer.ProduceAsync(_produceTopic, new Message<Null, string> { Value = json }, stoppingToken);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError("Ошибка обработки сообщения: {}", ex.Error.Reason);
            }
        }
    }

    /// <summary>
    /// Освобождает ресурсы, используемые сервисом, и корректно завершает работу с Kafka.
    /// </summary>
    public override void Dispose()
    {
        _consumer?.Close();
        _consumer?.Dispose();
        _producer?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}