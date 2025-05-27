using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

public interface IDiscInfoRepository
{
    DiscInfo? GetByType(DiscType type);
    List<DiscInfo> GetAll();
}