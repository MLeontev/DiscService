using DiscService.Bot.Messaging.Models;
using DiscService.Bot.UI;
using DiscService.Core.Interfaces;

namespace DiscService.Core.Services;

/// <summary>
/// Реализация <see cref="IDiscInfoService"/> для получения информации о типах DISC.
/// </summary>
public class DiscInfoService : IDiscInfoService
{
    private readonly IDiscInfoRepository _discInfoRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DiscInfoService"/>.
    /// </summary>
    /// <param name="discInfoRepository">Репозиторий информации о типах DISC.</param>
    public DiscInfoService(IDiscInfoRepository discInfoRepository)
    {
        _discInfoRepository = discInfoRepository;
    }

    /// <inheritdoc />
    public BotMessage GetDiscInfo(string chatId, Guid kafkaMessageId)
    {
        var infos = _discInfoRepository.GetAll();
        var responseText = MessageFormatter.FormatDiscInfo(infos);
        return BotMessage.Create(chatId, kafkaMessageId, responseText);
    }
}