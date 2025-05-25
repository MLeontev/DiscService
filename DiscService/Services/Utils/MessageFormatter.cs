using System.Text;
using DiscService.Models;

namespace DiscService.Services.Utils;

public static class MessageFormatter
{
    public static string FormatQuestion(Question question)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"*Вопрос {question.Number}*:");
        sb.AppendLine($"{question.Text}");
        foreach (var option in question.Answers)
        {
            sb.AppendLine($"\n*{option.Label})* {option.Text}");
        }

        return sb.ToString();
    }

    public static string FormatResult(TestResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{DiscType.Dominance.ToEmoji()} D: {result.DominanceScore}");
        sb.AppendLine($"{DiscType.Influence.ToEmoji()} I: {result.InfluenceScore}");
        sb.AppendLine($"{DiscType.Steadiness.ToEmoji()} S: {result.SteadinessScore}");
        sb.AppendLine($"{DiscType.Compliance.ToEmoji()} C: {result.ComplianceScore}");
        return sb.ToString();
    }

    public static string FormatDiscInfo(List<DiscInfo> infos)
    {
        var sb = new StringBuilder();

        foreach (var info in infos)
        {
            sb.AppendLine($"{info.DiscType.ToEmoji()} *{info.DiscType.ToString()[0]}-доминанта* ({info.DiscName})");
            sb.AppendLine($"_Ключевые слова: {string.Join(", ", info.Keywords)}_");
            sb.AppendLine(info.Description);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static string FormatComparison(TestResult previous, TestResult current)
    {
        var sb = new StringBuilder();
        sb.AppendLine("*Сравнение последних результатов:*");
        sb.AppendLine($"_Предыдущий: {previous.FinishedAt:dd.MM.yyyy}_");
        sb.AppendLine($"_Текущий: {current.FinishedAt:dd.MM.yyyy}_");
        sb.AppendLine();

        sb.AppendLine($"{DiscType.Dominance.ToEmoji()} *D*: {DiffText(previous.DominanceScore, current.DominanceScore)}");
        sb.AppendLine($"{DiscType.Influence.ToEmoji()} *I*: {DiffText(previous.InfluenceScore, current.InfluenceScore)}");
        sb.AppendLine($"{DiscType.Steadiness.ToEmoji()} *S*: {DiffText(previous.SteadinessScore, current.SteadinessScore)}");
        sb.AppendLine($"{DiscType.Compliance.ToEmoji()} *C*: {DiffText(previous.ComplianceScore, current.ComplianceScore)}");

        return sb.ToString();
    }

    private static string DiffText(int oldScore, int newScore)
    {
        var diff = newScore - oldScore;

        if (diff == 0)
            return $"{newScore} (без изменений)";
        var arrow = diff > 0 ? "⬆️" : "⬇️";
        return $"{oldScore} → {newScore} ({arrow} {Math.Abs(diff)})";
    }
}