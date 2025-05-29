namespace DiscService.Core.Models;

/// <summary>
/// Представляет результат прохождения DISC-теста.
/// </summary>
public class TestResult
{
    /// <summary>
    /// Id результата теста (ключ в БД).
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Id чата Telegram, к которому относится результат.
    /// </summary>
    public string ChatId { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата и время завершения теста.
    /// </summary>
    public DateTime FinishedAt { get; set; }
    
    /// <summary>
    /// Количество баллов, полученных по психотипу D.
    /// </summary>
    public int DominanceScore { get; set; }
    
    /// <summary>
    /// Количество баллов, полученных по психотипу I.
    /// </summary>
    public int InfluenceScore { get; set; }
    
    /// <summary>
    /// Количество баллов, полученных по психотипу S.
    /// </summary>
    public int SteadinessScore { get; set; }
    
    /// <summary>
    /// Количество баллов, полученных по психотипу C.
    /// </summary>
    public int ComplianceScore { get; set; }
}