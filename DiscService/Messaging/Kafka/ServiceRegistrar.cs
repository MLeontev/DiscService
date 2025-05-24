using System.Text.Json;
using Confluent.Kafka;
using DiscService.Messaging.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscService.Messaging.Kafka;

public class ServiceRegistrar : IServiceRegistrar
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<ServiceRegistrar> _logger;

    public ServiceRegistrar(
        IOptions<KafkaSettings> kafkaSettings, 
        IProducer<Null, string> producer, 
        ILogger<ServiceRegistrar> logger)
    {
        _producer = producer;
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
    }

    public async Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken)
    {
        var registrationRequest = new ServiceRegistrationRequest
        {
            Name = _kafkaSettings.ServiceName,
            Description = "Описание сервиса",
            Commands =
            [
                new CommandInfo("/hello", "Описание команды1", "ADD", "ANONYMOUS"),
                new CommandInfo("/world", "Описание команды2", "ADD", "ANONYMOUS"), 
                new CommandInfo("/buttons", "Описание команды3", "ADD", "ANONYMOUS"),
                new CommandInfo("callback_test", "Описание callback123", "ADD", "ANONYMOUS")
            ]
        };

        var requestJson = JsonSerializer.Serialize(registrationRequest);
        
        await _producer.ProduceAsync(
            _kafkaSettings.InfoRequestTopic, 
            new Message<Null, string> { Value = requestJson }, 
            stoppingToken);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = $"{_kafkaSettings.ServiceName}-registration-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(_kafkaSettings.InfoResponseTopic);

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
            finally
            {
                consumer.Close();
            }
        }
        
        throw new InvalidOperationException("Регистрация сервиса не завершена — не получен ответ от Kafka.");
    }
}