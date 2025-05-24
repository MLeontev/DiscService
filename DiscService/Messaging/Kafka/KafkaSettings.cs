namespace DiscService.Messaging.Kafka;

public class KafkaSettings
{
    public string ServiceName { get; set; } = string.Empty;
    public string BootstrapServers { get; set; } = string.Empty;
    public string InfoRequestTopic { get; set; } = string.Empty;
    public string InfoResponseTopic { get; set; } = string.Empty;
}