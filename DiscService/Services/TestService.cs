using DiscService.Constants;
using DiscService.Data;
using DiscService.Data.Repositories;
using DiscService.Messaging.Models;
using DiscService.Models;
using DiscService.Services.Utils;

namespace DiscService.Services;

public class TestService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly SessionManager _sessionManager;
    private readonly AppDbContext _dbContext;

    public TestService(
        IQuestionRepository questionRepository,
        SessionManager sessionManager,
        AppDbContext dbContext)
    {
        _questionRepository = questionRepository;
        _sessionManager = sessionManager;
        _dbContext = dbContext;
    }

    public BotMessage StartTest(string chatId, Guid kafkaMessageId)
    {
        const string greeting = $"Вы собираетесь пройти DISC-тест.\n\nВсего 15 вопросов. Отвечайте честно, выбрав утверждение, которое лучше всего вас описывает.\n\nВы можете прервать тест в любой момент, отправив {BotCommands.CancelTestCommand}.\n\nГотовы начать?";

        return BotMessage.Create(
            chatId,
            kafkaMessageId,
            greeting,
            KeyboardBuilder.BuildBeginTestKeyboard(),
            parseMode: null
        );
    }

    public BotMessage? BeginTest(string chatId, Guid kafkaMessageId)
    {
        var session = _sessionManager.CreateSession(chatId);
        session.CurrentQuestionNumber = 1;
        return GetQuestionMessage(chatId, 1, kafkaMessageId);
    }

    public async Task<BotMessage?> AnswerQuestion(string chatId, string callbackData, Guid kafkaMessageId)
    {
        var session = _sessionManager.GetSession(chatId);
        if (session == null)
            return BotMessage.Create(chatId, kafkaMessageId,
                $"Тест не начат или завершён. Отправьте {BotCommands.StartTestCommand}, чтобы начать заново", parseMode: null);

        if (!callbackData.StartsWith("disc_answer")) return null;

        var label = GetAnswerLabel(callbackData);
        if (label == null) return null;

        var currentQuestion = _questionRepository.GetByNumber(session.CurrentQuestionNumber);

        var selectedAnswer = currentQuestion?.Answers.FirstOrDefault(a => a.Label == label);
        if (selectedAnswer == null) return null;

        session.UserAnswers.Add(new UserAnswer(session.CurrentQuestionNumber, selectedAnswer.Label, selectedAnswer.DiscType));
        session.CurrentQuestionNumber++;

        if (session.CurrentQuestionNumber > _questionRepository.GetAll().Count)
            return await FinishTest(session, kafkaMessageId);

        return GetQuestionMessage(chatId, session.CurrentQuestionNumber, kafkaMessageId);
    }

    public BotMessage CancelTest(string chatId, Guid kafkaMessageId)
    {
        var session = _sessionManager.GetSession(chatId);
        if (session == null)
            return BotMessage.Create(chatId, kafkaMessageId,
                "Тест не начат или завершён", parseMode: null);

        _sessionManager.RemoveSession(chatId);
        
        return BotMessage.Create(chatId, kafkaMessageId,
            $"Тест отменен. Отправьте {BotCommands.StartTestCommand}, чтобы начать заново", parseMode: null);
    }

    private BotMessage? GetQuestionMessage(string chatId, int number, Guid kafkaMessageId)
    {
        var question = _questionRepository.GetByNumber(number);
        if (question == null) return null;

        var questionsCount = _questionRepository.GetAll().Count;

        return BotMessage.Create(chatId, kafkaMessageId,
            MessageFormatter.FormatQuestion(question, questionsCount),
            KeyboardBuilder.BuildAnswerKeyboard(question));
    }

    private string? GetAnswerLabel(string callbackData)
    {
        return callbackData.Replace(BotCommands.AnswerPrefix, "") switch
        {
            "A" => "А",
            "B" => "Б",
            "C" => "В",
            "D" => "Г",
            _ => null
        };
    }

    private async Task<BotMessage> FinishTest(UserSession session, Guid kafkaMessageId)
    {
        _sessionManager.RemoveSession(session.ChatId);

        var result = new TestResult
        {
            ChatId = session.ChatId,
            FinishedAt = DateTime.UtcNow,
            DominanceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Dominance),
            InfluenceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Influence),
            SteadinessScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Steadiness),
            ComplianceScore = session.UserAnswers.Count(a => a.SelectedCategory == DiscType.Compliance)
        };

        _dbContext.TestResults.Add(result);
        await _dbContext.SaveChangesAsync();

        var message = "Тест завершён! Ваш результат:\n" + MessageFormatter.FormatResult(result);

        return BotMessage.Create(session.ChatId, kafkaMessageId, message, KeyboardBuilder.BuildTestResultKeyboard());
    }
}
