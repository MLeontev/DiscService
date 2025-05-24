using System.Text.Json;
using Confluent.Kafka;
using DiscService.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscService.Messaging.Kafka;

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
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult == null) continue;
                
                var incomingMessage = JsonSerializer.Deserialize<BotMessage>(consumeResult.Message.Value);
                if (incomingMessage == null) continue;
                
                using var scope = _serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();
                var response = await handler.HandleAsync(incomingMessage);
                if (response != null)
                {
                    var json = JsonSerializer.Serialize(response);
                    await _producer.ProduceAsync(_produceTopic, new Message<Null, string> { Value = json }, stoppingToken);
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError("Ошибка обработки сообщения: {}", ex.Error.Reason);
            }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        _producer.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}