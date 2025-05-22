using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscService;

public class CommandWorkerService : BackgroundService
{
    private readonly ILogger<CommandWorkerService> _logger;
    private const string ServiceName = "MyNewService1";
    private const string BootstrapServers = "localhost:9093";
    
    public CommandWorkerService(ILogger<CommandWorkerService> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var registrationRequest = new
        {
            name = ServiceName,
            description = "Тестовая регистрация",
            commands = new[]
            {
                new
                {
                    name = "/hellonew",
                    description = "Приветственная команда",
                    action = "ADD",
                    right = "ANONYMOUS"
                }
            }
        };
        
        var producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers };
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        
        var requestJson = JsonSerializer.Serialize(registrationRequest);
        await producer.ProduceAsync("service-info-request", new Message<Null, string> { Value = requestJson }, stoppingToken);
        _logger.LogInformation("Регистрация отправлена в service-info-request");
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = "registration-check-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe("service-info-response");
        
        _logger.LogInformation("Ожидаем ответ в service-info-response...");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            try
            {
                var response = JsonSerializer.Deserialize<ServiceRegistrationResponse>(
                    consumeResult.Message.Value,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (response?.ServiceName == ServiceName)
                {
                    _logger.LogInformation("Сервис зарегистрирован!");
                    _logger.LogInformation("ConsumeTopic: {0}", response.ConsumeTopic);
                    _logger.LogInformation("ProduceTopic: {0}", response.ProduceTopic);
                    _logger.LogInformation("Message: {0}", response.Message);
                    break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Ошибка парсинга JSON: {0}", ex.Message);
            }
        }
        
        consumer.Close();
        _logger.LogInformation("Завершение регистрации.");
    }
    
    private class ServiceRegistrationResponse
    {
        public string ServiceName { get; set; }
        public string ConsumeTopic { get; set; }
        public string ProduceTopic { get; set; }
        public string Message { get; set; }
    }

}