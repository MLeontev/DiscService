using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс репозитория для получения информации о типах DISC.
/// </summary>
public interface IDiscInfoRepository
{
    /// <summary>
    /// Возвращает информацию о типе DISC по значению.
    /// </summary>
    /// <param name="type">Тип DISC.</param>
    /// <returns>Объект <see cref="DiscInfo"/> или <c>null</c>, если не найдено.</returns>
    DiscInfo? GetByType(DiscType type);
    
    /// <summary>
    /// Возвращает список всей информации по типам DISC.
    /// </summary>
    /// <returns>Список всех объектов <see cref="DiscInfo"/>.</returns>
    List<DiscInfo> GetAll();
}