using DiscService.Bot.Messaging.Models;
using DiscService.Bot.UI;
using DiscService.Core.Interfaces;
using DiscService.Data.Repositories;

namespace DiscService.Core.Services;

public class DiscInfoService : IDiscInfoService
{
    private readonly IDiscInfoRepository _discInfoRepository;

    public DiscInfoService(IDiscInfoRepository discInfoRepository)
    {
        _discInfoRepository = discInfoRepository;
    }

    public BotMessage GetDiscInfo(string chatId, Guid kafkaMessageId)
    {
        var infos = _discInfoRepository.GetAll();
        var responseText = MessageFormatter.FormatDiscInfo(infos);
        return BotMessage.Create(chatId, kafkaMessageId, responseText);
    }
}