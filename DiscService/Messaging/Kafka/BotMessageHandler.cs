using DiscService.Data.Repositories;
using DiscService.Messaging.Models;
using DiscService.Models;
using DiscService.Services;

namespace DiscService.Messaging.Kafka;

public class BotMessageHandler : IMessageHandler
{
    private readonly IDiscInfoRepository _discInfoRepository;
    private readonly TestService _testService;

    public BotMessageHandler(
        IDiscInfoRepository discInfoRepository, 
        TestService testService)
    {
        _discInfoRepository = discInfoRepository;
        _testService = testService;
    }

    public Task<BotMessage?> HandleAsync(BotMessage incoming)
    {
        string? responseText;
        object? replyMarkup = null;
        
        if (incoming.Data.Text != null && incoming.Data.Text.StartsWith("disc_answer_"))
        {
            return Task.FromResult(_testService.HandleAnswer(incoming.Data.ChatId!, incoming.Data.Text, incoming.KafkaMessageId));
        }

        switch (incoming.Data.Text)
        {
            case "/disc_info":
                var allInfos = _discInfoRepository.GetAll();
                responseText = string.Join("\n\n", allInfos.Select(info =>
                    $"{info.DiscType.ToEmoji()} *{info.DiscType.ToString()[0]}-доминанта* ({info.DiscName})\n_Ключевые слова: {string.Join(", ", info.Keywords)}_\n{info.Description}"));
                break;
            
            case "/start_test":
                return Task.FromResult(_testService.HandleStartTest(incoming.Data.ChatId!, incoming.KafkaMessageId));
            
            default:
                responseText = "Неизвестная команда";
                break;
        }

        return Task.FromResult<BotMessage?>(new BotMessage
        {
            Method = "sendmessage",
            Filename = null,
            KafkaMessageId = incoming.KafkaMessageId,
            Status = "COMPLETED",
            Data = new SendMessageData
            {
                ChatId = incoming.Data.ChatId,
                Text = responseText,
                ParseMode = "Markdown",
                ReplyMarkup = replyMarkup
            }
        });
    }
}