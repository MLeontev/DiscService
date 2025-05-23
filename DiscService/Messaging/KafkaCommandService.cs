using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscService.Messaging;

public class KafkaCommandService : BackgroundService
{
    private const string ServiceName = "DiscService";
    private const string BootstrapServers = "localhost:9093";
    
    private readonly ILogger<KafkaCommandService> _logger;

    private IConsumer<Ignore, string> _consumer;
    private IProducer<Null, string> _producer;

    private string _consumeTopic;
    private string _produceTopic;

    public KafkaCommandService(ILogger<KafkaCommandService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RegisterServiceAsync(stoppingToken);

        var producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = $"{ServiceName}-group",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _consumer.Subscribe(_consumeTopic);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult != null) await HandleAsync(consumeResult.Message.Value);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError("Ошибка обработки сообщения: {}", ex.Error.Reason);
            }
    }

    private async Task RegisterServiceAsync(CancellationToken stoppingToken)
    {
        var registrationRequest = new ServiceRegistrationRequest
        {
            Name = ServiceName,
            Description = "Описание сервиса",
            Commands =
            [
                new CommandInfo
                    { Name = "/hello", Description = "Описание команды1", Action = "ADD", Right = "ANONYMOUS" },
                new CommandInfo
                    { Name = "/world", Description = "Описание команды2", Action = "ADD", Right = "ANONYMOUS" },
                new CommandInfo
                    { Name = "/buttons", Description = "Описание команды3", Action = "ADD", Right = "ANONYMOUS" },
                new CommandInfo 
                    { Name = "callback_test", Description = "Описание callback123", Action = "ADD", Right = "ANONYMOUS" }
            ]
        };

        var requestJson = JsonSerializer.Serialize(registrationRequest);

        var producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers };
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        await producer.ProduceAsync("service-info-request", new Message<Null, string> { Value = requestJson },
            stoppingToken);

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = $"{ServiceName}-registration-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe("service-info-response");

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var response = JsonSerializer.Deserialize<ServiceRegistrationResponse>(
                    consumeResult.Message.Value,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response?.ServiceName == ServiceName)
                {
                    _consumeTopic = response.ConsumeTopic;
                    _produceTopic = response.ProduceTopic;
                    break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Ошибка парсинга JSON: {}", ex.Message);
            }

        consumer.Close();
    }

    private async Task HandleAsync(string value)
    {
        try
        {
            var incoming = JsonSerializer.Deserialize<BotMessage>(value, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (incoming == null)
            {
                _logger.LogWarning("Пустое сообщение");
                return;
            }

            string? responseText = null;
            object? replyMarkup = null;
            
            if (incoming.Data.Text != null)
                switch (incoming.Data.Text)
                {
                    case "/hello":
                        responseText = "Привет! Это *DiscService* 👋";
                        break;
                    case "/world":
                        responseText = "Команда /world _принята_ 🌍";
                        break;
                    case "callback_test":
                        responseText = $"Вы нажали: *{incoming.Data.Text}*";
                        break;
                    case "/buttons":
                        responseText = "Нажмите кнопку:";
                        replyMarkup = new
                        {
                            inline_keyboard = new[]
                            {
                                new[]
                                {
                                    new
                                    {
                                        text = "👉 Нажми меня",
                                        callback_data = "callback_test"
                                    }
                                }
                            }
                        };
                        break;
                    default:
                        responseText = "Неизвестная команда.";
                        break;
                }

            if (responseText == null)
            {
                _logger.LogWarning("Неизвестный формат сообщения");
                return;
            }

            var sendData = new SendMessageData
            {
                ChatId = incoming.Data.ChatId,
                Text = responseText,
                ParseMode = "Markdown",
                ReplyMarkup = replyMarkup
            };

            var botMessage = new BotMessage
            {
                Method = "sendmessage",
                Filename = null,
                Data = sendData,
                KafkaMessageId = incoming.KafkaMessageId,
                Status = "COMPLETED"
            };

            var json = JsonSerializer.Serialize(botMessage);

            await _producer.ProduceAsync(_produceTopic, new Message<Null, string> { Value = json });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке сообщения Kafka");
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

public class ServiceRegistrationResponse
{
    [JsonPropertyName("serviceName")]public string ServiceName { get; set; }

    [JsonPropertyName("consumeTopic")] public string ConsumeTopic { get; set; }

    [JsonPropertyName("produceTopic")] public string ProduceTopic { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
}

public class CommandInfo
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("action")] public string Action { get; set; }

    [JsonPropertyName("right")] public string Right { get; set; }
}

public class ServiceRegistrationRequest
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("commands")] public List<CommandInfo> Commands { get; set; }
}

public class BotMessage
{
    [JsonPropertyName("method")] public string? Method { get; set; }

    [JsonPropertyName("filename")] public string? Filename { get; set; }

    [JsonPropertyName("data")] public SendMessageData Data { get; set; } // объект типа SendMessage и т.п.

    [JsonPropertyName("kafkaMessageId")] public Guid KafkaMessageId { get; set; }

    [JsonPropertyName("status")] public string? Status { get; set; } // "IN_PROGRESS", "COMPLETED", и т.д.
}

public class SendMessageData
{
    [JsonPropertyName("chat_id")] public string? ChatId { get; set; }

    [JsonPropertyName("text")] public string? Text { get; set; }

    [JsonPropertyName("method")] public string Method { get; set; } = "sendmessage";

    [JsonPropertyName("parse_mode")] public string? ParseMode { get; set; }

    [JsonPropertyName("reply_markup")] public object? ReplyMarkup { get; set; }
}