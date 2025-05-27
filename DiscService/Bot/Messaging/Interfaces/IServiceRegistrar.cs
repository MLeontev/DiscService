namespace DiscService.Bot.Messaging.Interfaces;

public interface IServiceRegistrar
{
    Task<(string consumeTopic, string produceTopic)> RegisterAsync(CancellationToken stoppingToken);
}