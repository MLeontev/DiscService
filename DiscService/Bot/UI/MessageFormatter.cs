using System.Text;
using DiscService.Core.Models;

namespace DiscService.Bot.UI;

public static class MessageFormatter
{
    public static string FormatQuestion(Question question, int questionsCount)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"*Вопрос {question.Number} из {questionsCount}*:");
        sb.AppendLine($"{question.Text}");
        foreach (var option in question.Answers)
        {
            sb.AppendLine($"\n*{option.Label})* {option.Text}");
        }
        
        sb.AppendLine();
        sb.AppendLine("👇 Выберите вариант ниже");

        return sb.ToString();
    }

    public static string FormatResult(TestResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"*{DiscType.Dominance.ToEmoji()} D:* {result.DominanceScore}");
        sb.AppendLine($"*{DiscType.Influence.ToEmoji()} I:* {result.InfluenceScore}");
        sb.AppendLine($"*{DiscType.Steadiness.ToEmoji()} S:* {result.SteadinessScore}");
        sb.AppendLine($"*{DiscType.Compliance.ToEmoji()} C:* {result.ComplianceScore}");
        sb.AppendLine();
        
        var scores = new Dictionary<DiscType, int>
        {
            { DiscType.Dominance, result.DominanceScore },
            { DiscType.Influence, result.InfluenceScore },
            { DiscType.Steadiness, result.SteadinessScore },
            { DiscType.Compliance, result.ComplianceScore }
        };
        
        var maxScore = scores.Max(x => x.Value);
        
        var mainTypes = scores
            .Where(kvp => kvp.Value == maxScore)
            .Select(kvp => kvp.Key)
            .ToList();

        if (mainTypes.Count == scores.Count)
        {
            sb.AppendLine("*🧠 Ваш результат показывает равномерное распределение всех четырёх типов DISC.*");
        }
        else if (mainTypes.Count == 1)
        {
            var type = mainTypes.First();
            sb.AppendLine($"*Ваш доминирующий тип — {type.ToEmoji()} {type.ToString()[0]}*");
        }
        else
        {
            var types = string.Join(", ", mainTypes.Select(t => $"{t.ToEmoji()} {t.ToString()[0]}"));
            sb.AppendLine($"*Ваши доминирующие типы — {types}*");
        }
        
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

        var changeSymbol = diff > 0 ? "+" : "-";

        return $"{oldScore} → {newScore} ({changeSymbol}{Math.Abs(diff)})";
    }
}