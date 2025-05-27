using Confluent.Kafka;
using DiscService.Bot.Messaging.Interfaces;
using DiscService.Bot.Messaging.Kafka;
using DiscService.Core.Interfaces;
using DiscService.Core.Services;
using DiscService.Data;
using DiscService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DiscService;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<KafkaCommandService>();
                services.Configure<KafkaSettings>(context.Configuration.GetSection("KafkaSettings"));
                services.AddSingleton<IProducer<Null, string>>(sp =>
                {
                    var kafkaSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
                    var config = new ProducerConfig { BootstrapServers = kafkaSettings.BootstrapServers };
                    return new ProducerBuilder<Null, string>(config).Build();
                });
                
                services.AddSingleton<IServiceRegistrar, ServiceRegistrar>();
                services.AddScoped<IMessageHandler, BotMessageHandler>();

                services.AddSingleton<SessionManager>();
                services.AddScoped<TestService>();
                services.AddScoped<DiscInfoService>();
                services.AddScoped<ResultService>();

                services.AddSingleton<IDiscInfoRepository, InMemoryDiscInfoRepository>();
                services.AddSingleton<IQuestionRepository, InMemoryQuestionRepository>();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            })
            .Build();

        await host.RunAsync();
    }
}