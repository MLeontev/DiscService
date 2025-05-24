using DiscService.Models;

namespace DiscService.Data.Repositories;

public class InMemoryDiscInfoRepository : IDiscInfoRepository
{
    private readonly Dictionary<DiscType, DiscInfo> _discInfos;

    public InMemoryDiscInfoRepository()
    {
        _discInfos = new Dictionary<DiscType, DiscInfo>
        {
            [DiscType.Dominance] = new(
                DiscType.Dominance,
                "Доминирование",
                ["Революция", "Страсть", "Движение", "Огонь"],
                "Люди с выраженной D-доминантой — волевые, амбициозные, решительные, жёсткие, твёрдые, смелые, быстро принимающие решения. " +
                "В условиях высокой неопределённости остаются функциональными, умеют брать на себя ответственность и продолжают действовать, не впадая в ступор или размышления."),
            
            [DiscType.Influence] = new(
                DiscType.Influence,
                "Влияние",
                ["Тепло", "Солнце", "Пляж", "Веселье"],
                "Люди с развитой жёлтой доминантой открыты, харизматичны, коммуникабельны, оптимистичны."),
            
            [DiscType.Steadiness] = new(
                DiscType.Steadiness,
                "Постоянство",
                ["Жизнь", "Трава", "Дерево", "Спокойствие"],
                "Стабильны, уравновешены, обходительны, хорошие слушатели. Эмпатия, понимание — естественные вещи для них. " +
                "Боятся обидеть, потревожить, побеспокоить или расстроить окружающих."),
            
            [DiscType.Compliance] = new(
                DiscType.Compliance,
                "Следование правилам",
                ["Океан", "Лед", "Холодный разум"],
                "Люди-аналитики, точные, пунктуальные, системные, логичные, самодисциплинированные. " +
                "Решения принимают медленно, долго взвешивают и обдумывают. " +
                "Для эффективности работы порой их стоит выводить из состояния обдумывания, чтобы призвать к реальным действиям.")
        };
    }
    
    public DiscInfo GetByType(DiscType type)
    {
        return _discInfos[type];
    }

    public List<DiscInfo> GetAll()
    {
        return _discInfos.Values.ToList();
    }
}