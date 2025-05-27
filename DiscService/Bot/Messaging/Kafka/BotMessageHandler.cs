using DiscService.Bot.Commands;
using DiscService.Bot.Messaging.Interfaces;
using DiscService.Bot.Messaging.Models;
using DiscService.Core.Services;

namespace DiscService.Bot.Messaging.Kafka;

public class BotMessageHandler : IMessageHandler
{
    private readonly TestService _testService;
    private readonly DiscInfoService _discInfoService;
    private readonly ResultService _resultService;
    private readonly SessionManager _sessionManager;

    public BotMessageHandler(
        TestService testService,
        DiscInfoService discService,
        ResultService resultService, 
        SessionManager sessionManager)
    {
        _testService = testService;
        _discInfoService = discService;
        _resultService = resultService;
        _sessionManager = sessionManager;
    }

    public async Task<BotMessage?> HandleAsync(BotMessage incoming)
    {
        if (incoming.Data.ChatId == null || incoming.Data.Text == null) return null;

        var chatId = incoming.Data.ChatId;
        var text = incoming.Data.Text;
        var messageId = incoming.KafkaMessageId;
        
        if (text.StartsWith(BotCommands.AnswerPrefix))
            return await _testService.AnswerQuestion(chatId, text, messageId);
        
        if (text == BotCommands.CancelTestCommand)
            return _testService.CancelTest(chatId, messageId);
        
        if (_sessionManager.HasSession(chatId))
            return RestrictCommandDuringTest(chatId, messageId);

        return incoming.Data.Text switch
        {
            BotCommands.StartTestCommand => _testService.StartTest(incoming.Data.ChatId, incoming.KafkaMessageId),
            BotCommands.BeginTestCallback => _testService.BeginTest(incoming.Data.ChatId, incoming.KafkaMessageId),

            BotCommands.LastResultCommand => await _resultService.GetLastResultAsync(incoming.Data.ChatId, incoming.KafkaMessageId),
            
            BotCommands.CompareResultsCommand => await _resultService.CompareResults(incoming.Data.ChatId, incoming.KafkaMessageId),
            BotCommands.CompareResultsCallback => await _resultService.CompareResults(incoming.Data.ChatId, incoming.KafkaMessageId),

            BotCommands.GetInfoCommand => _discInfoService.GetDiscInfo(incoming.Data.ChatId, incoming.KafkaMessageId),
            BotCommands.GetInfoCallback => _discInfoService.GetDiscInfo(incoming.Data.ChatId, incoming.KafkaMessageId),

            _ => BotMessage.Create(chatId, messageId, "Неизвестная команда.", parseMode: null)
        };
    }
    
    private BotMessage RestrictCommandDuringTest(string chatId, Guid kafkaMessageId)
    {
        return BotMessage.Create(
            chatId,
            kafkaMessageId,
            $"Вы проходите тест. Завершите или отмените его прежде чем использовать другие команды.\n\nЧтобы прервать тест, используйте {BotCommands.CancelTestCommand}",
            parseMode: null);
    }
}