using DiscService.Core.Interfaces;
using DiscService.Core.Models;

namespace DiscService.Data.Repositories;

/// <summary>
/// Реализация <see cref="IDiscInfoRepository"/>, использующая in-memory словарь.
/// </summary>
public class InMemoryDiscInfoRepository : IDiscInfoRepository
{
    private static readonly Dictionary<DiscType, DiscInfo> DiscInfos = new()
    {
        [DiscType.Dominance] = new DiscInfo(
            DiscType.Dominance,
            "Доминирование",
            ["Революция", "Страсть", "Движение", "Огонь"],
            "Люди с выраженной D-доминантой — волевые, амбициозные, решительные, жёсткие, твёрдые, смелые, быстро принимающие решения. " +
            "В условиях высокой неопределённости остаются функциональными, умеют брать на себя ответственность и продолжают действовать, не впадая в ступор или размышления."),
            
        [DiscType.Influence] = new DiscInfo(
            DiscType.Influence,
            "Влияние",
            ["Тепло", "Солнце", "Пляж", "Веселье"],
            "Люди с развитой жёлтой доминантой открыты, харизматичны, коммуникабельны, оптимистичны."),
            
        [DiscType.Steadiness] = new DiscInfo(
            DiscType.Steadiness,
            "Постоянство",
            ["Жизнь", "Трава", "Дерево", "Спокойствие"],
            "Стабильны, уравновешены, обходительны, хорошие слушатели. Эмпатия, понимание — естественные вещи для них. " +
            "Боятся обидеть, потревожить, побеспокоить или расстроить окружающих."),
            
        [DiscType.Compliance] = new DiscInfo(
            DiscType.Compliance,
            "Следование правилам",
            ["Океан", "Лед", "Холодный разум"],
            "Люди-аналитики, точные, пунктуальные, системные, логичные, самодисциплинированные. " +
            "Решения принимают медленно, долго взвешивают и обдумывают. " +
            "Для эффективности работы порой их стоит выводить из состояния обдумывания, чтобы призвать к реальным действиям.")
    };

    /// <inheritdoc />
    public DiscInfo GetByType(DiscType type)
    {
        return DiscInfos[type];
    }

    /// <inheritdoc />
    public List<DiscInfo> GetAll()
    {
        return DiscInfos.Values.ToList();
    }
}