using System.Text;
using DiscService.Data.Repositories;
using DiscService.Messaging.Models;
using DiscService.Models;

namespace DiscService.Services;

public class QuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly SessionManager _sessionManager;

    public QuestionService(
        IQuestionRepository questionRepository, 
        SessionManager sessionManager)
    {
        _questionRepository = questionRepository;
        _sessionManager = sessionManager;
    }

    public BotMessage? GetQuestionMessage(string chatId, int number)
    {
        // получить или создать сессию
        
        // получить номер вопроса
        
        // если последний то выдать результат
        
        // иначе выдать вопрос, сдвинуть номер вопроса
        
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
            }
        };
    }

    private static object GenerateAnswerButtons(Question question)
    {
        var buttons = question.Answers
            .Select(a => new
            {
                a.Label, 
                Callback = $"answer_{a.Label}"
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
}