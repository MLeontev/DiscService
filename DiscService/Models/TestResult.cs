namespace DiscService.Models;

public class TestResult
{
    public int Id { get; set; }
    public string ChatId { get; set; } = string.Empty;
    public DateTime FinishedAt { get; set; }
    public int DominanceScore { get; set; }
    public int InfluenceScore { get; set; }
    public int SteadinessScore { get; set; }
    public int ComplianceScore { get; set; }
}