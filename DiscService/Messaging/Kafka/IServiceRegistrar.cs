namespace DiscService.Messaging.Kafka;

public interface IServiceRegistrar
{
    Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken);
}