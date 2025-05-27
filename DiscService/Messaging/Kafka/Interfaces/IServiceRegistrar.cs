namespace DiscService.Messaging.Kafka.Interfaces;

public interface IServiceRegistrar
{
    Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken);
}