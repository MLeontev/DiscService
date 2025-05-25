using DiscService.Data.Repositories;
using DiscService.Messaging.Models;
using DiscService.Services.Utils;

namespace DiscService.Services;

public class DiscInfoService
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