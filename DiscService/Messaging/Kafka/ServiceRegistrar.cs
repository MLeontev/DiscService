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
                new CommandInfo("/disc_info", "Получить описание психотипов по DISC", "ADD", "ANONYMOUS"),
                new CommandInfo("/start_test", "Начать DISC-тестирование", "ADD", "ANONYMOUS"),
                new CommandInfo("disc_answer_A", "Ответ A на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo("disc_answer_B", "Ответ Б на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo("disc_answer_C", "Ответ В на текущий вопрос DISC-теста", "ADD", "ANONYMOUS"),
                new CommandInfo("disc_answer_D", "Ответ Г на текущий вопрос DISC-теста", "ADD", "ANONYMOUS")
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
        }
        
        throw new InvalidOperationException("Регистрация сервиса не завершена — не получен ответ от Kafka.");
    }
}