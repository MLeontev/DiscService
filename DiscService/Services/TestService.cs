using System.Text;
using DiscService.Data.Repositories;
using DiscService.Messaging.Models;
using DiscService.Models;

namespace DiscService.Services;

public class TestService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly SessionManager _sessionManager;

    public TestService(
        IQuestionRepository questionRepository, 
        SessionManager sessionManager)
    {
        _questionRepository = questionRepository;
        _sessionManager = sessionManager;
    }

    public BotMessage? HandleStartTest(string chatId, Guid kafkaMessageId)
    {
        var session = _sessionManager.CreateSession(chatId);
        session.CurrentQuestionNumber = 1;
        
        return GetQuestionMessage(chatId, 1, kafkaMessageId);
    }

    public BotMessage? HandleAnswer(string chatId, string callbackData, Guid kafkaMessageId)
    {
        var session = _sessionManager.GetSession(chatId);
        if (session == null || !callbackData.StartsWith("disc_answer"))
            return null;

        var label = callbackData.Replace("disc_answer_", "") switch
        {
            "A" => "А",
            "B" => "Б",
            "C" => "В",
            "D" => "Г",
            _ => null
        };
        
        var currentQuestion = _questionRepository.GetByNumber(session.CurrentQuestionNumber);

        var selectedAnswer = currentQuestion?.Answers.FirstOrDefault(a => a.Label == label);
        if (selectedAnswer == null) return null;
        
        session.UserAnswers.Add(new UserAnswer(
            session.CurrentQuestionNumber, 
            selectedAnswer.Label, 
            selectedAnswer.DiscType));
        
        session.CurrentQuestionNumber++;

        if (session.CurrentQuestionNumber > _questionRepository.GetAll().Count)
        {
            _sessionManager.RemoveSession(chatId);
            
            var result = new TestResult
            {
                ChatId = chatId,
                FinishedAt = DateTime.UtcNow,
                DominanceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Dominance),
                InfluenceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Influence),
                SteadinessScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Steadiness),
                ComplianceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Compliance)
            };
            
            return new BotMessage
            {
                Method = "sendmessage",
                Status = "COMPLETED",
                Data = new SendMessageData
                {
                    ChatId = chatId,
                    Text = FormatResult(result),
                    ParseMode = "Markdown"
                },
                KafkaMessageId = kafkaMessageId
            };
        }
        
        return GetQuestionMessage(chatId, session.CurrentQuestionNumber, kafkaMessageId);
    }
    
    private BotMessage? GetQuestionMessage(string chatId, int number, Guid kafkaMessageId)
    {
        var question = _questionRepository.GetByNumber(number);
        if (question == null) return null;
        
        var questionMessage = FormatQuestionMessage(question);
        var replyMarkup = GenerateAnswerButtons(question);
        
        return new BotMessage
        {
            Method = "sendmessage",
            Status = "COMPLETED",
            Data = new SendMessageData
            {
                ChatId = chatId,
                Text = questionMessage,
                ParseMode = "Markdown",
                ReplyMarkup = replyMarkup
            },
            KafkaMessageId = kafkaMessageId
        };
    }

    private static object GenerateAnswerButtons(Question question)
    {
        var labelMap = new Dictionary<string, string>
        {
            ["А"] = "A",
            ["Б"] = "B",
            ["В"] = "C",
            ["Г"] = "D"
        };
        
        var buttons = question.Answers
            .Select(a => new
            {
                a.Label, 
                Callback = $"disc_answer_{labelMap[a.Label]}"
            })
            .Chunk(2)
            .Select(chunk => chunk
                .Select(a => new
                {
                    text = a.Label, 
                    callback_data = a.Callback
                }).ToArray())
            .ToArray();
        
        return new
        {
            inline_keyboard = buttons
        };
    }

    private static string FormatQuestionMessage(Question question)
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
    
    private static string FormatResult(TestResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{DiscType.Dominance.ToEmoji()} D: {result.DominanceScore}");
        sb.AppendLine($"{DiscType.Influence.ToEmoji()} I: {result.InfluenceScore}");
        sb.AppendLine($"{DiscType.Steadiness.ToEmoji()} S: {result.SteadinessScore}");
        sb.AppendLine($"{DiscType.Compliance.ToEmoji()} C: {result.ComplianceScore}");
        return sb.ToString();
    }
}