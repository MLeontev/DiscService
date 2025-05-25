using DiscService.Messaging.Models;
using DiscService.Services;

namespace DiscService.Messaging.Kafka;

public class BotMessageHandler : IMessageHandler
{
    private readonly TestService _testService;
    private readonly DiscInfoService _discInfoService;
    private readonly ResultService _resultService;

    public BotMessageHandler(
        TestService testService,
        DiscInfoService discService,
        ResultService resultService)
    {
        _testService = testService;
        _discInfoService = discService;
        _resultService = resultService;
    }

    public async Task<BotMessage?> HandleAsync(BotMessage incoming)
    {
        if (incoming.Data.ChatId == null || incoming.Data.Text == null) return null;

        if (incoming.Data.Text.StartsWith("disc_answer_"))
        {
            return await _testService.HandleAnswer(incoming.Data.ChatId, incoming.Data.Text, incoming.KafkaMessageId);
        }

        return incoming.Data.Text switch
        {
            "/disc_info" => _discInfoService.GetDiscInfo(incoming.Data.ChatId, incoming.KafkaMessageId),
            "/start_disc_test" => _testService.HandleStartTest(incoming.Data.ChatId, incoming.KafkaMessageId),
            "/disc_result" => await _resultService.GetLastResultAsync(incoming.Data.ChatId, incoming.KafkaMessageId),
            "/compare_disc_result" => await _resultService.CompareResults(incoming.Data.ChatId, incoming.KafkaMessageId),
            "disc_info" => _discInfoService.GetDiscInfo(incoming.Data.ChatId, incoming.KafkaMessageId),

            _ => BotMessage.Create(
                incoming.Data.ChatId,
                incoming.KafkaMessageId,
                "Неизвестная команда",
                parseMode: null),
        };
    }
}