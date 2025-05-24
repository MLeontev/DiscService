using DiscService.Models;

namespace DiscService.Data.Repositories;

public interface IDiscInfoRepository
{
    DiscInfo? GetByType(DiscType type);
    List<DiscInfo> GetAll();
}