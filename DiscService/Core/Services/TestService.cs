using DiscService.Bot.Commands;
using DiscService.Bot.Messaging.Models;
using DiscService.Bot.UI;
using DiscService.Core.Interfaces;
using DiscService.Core.Models;
using DiscService.Data;
using DiscService.Data.Repositories;

namespace DiscService.Core.Services;

/// <summary>
/// Реализация <see cref="ITestService"/> для управления прохождением DISC-теста.
/// </summary>
public class TestService : ITestService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ISessionManager _sessionManager;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestService"/>.
    /// </summary>
    /// <param name="questionRepository">Репозиторий вопросов теста.</param>
    /// <param name="sessionManager">Менеджер сессий пользователей.</param>
    /// <param name="dbContext">Контекст базы данных.</param>
    public TestService(
        IQuestionRepository questionRepository,
        ISessionManager sessionManager,
        AppDbContext dbContext)
    {
        _questionRepository = questionRepository;
        _sessionManager = sessionManager;
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public BotMessage StartTest(string chatId, Guid kafkaMessageId)
    {
        var questionCount = _questionRepository.GetCount();
        if (questionCount == 0)
            return BotMessage.Create(chatId, kafkaMessageId,
                "Сейчас тест не доступен. Попробуйте позже", parseMode: null);

        var greeting = $"Вы собираетесь пройти DISC-тест.\n\n🧠 Всего {questionCount} вопросов. Отвечайте честно, выбрав утверждение, которое лучше всего вас описывает.\n\nВы можете прервать тест в любой момент, отправив {BotCommands.CancelTestCommand}.\n\nГотовы начать?";

        return BotMessage.Create(
            chatId,
            kafkaMessageId,
            greeting,
            KeyboardBuilder.BuildBeginTestKeyboard(),
            parseMode: null
        );
    }

    /// <inheritdoc />
    public BotMessage? BeginTest(string chatId, Guid kafkaMessageId)
    {
        var session = _sessionManager.CreateSession(chatId);
        session.CurrentQuestionNumber = 1;
        return GetQuestionMessage(chatId, 1, kafkaMessageId);
    }

    /// <inheritdoc />
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

        session.UserAnswers.Add(new UserAnswer(
            session.CurrentQuestionNumber, 
            selectedAnswer.Label, 
            selectedAnswer.DiscTypes.ToArray()));
        session.CurrentQuestionNumber++;

        if (session.CurrentQuestionNumber > _questionRepository.GetCount())
            return await FinishTest(session, kafkaMessageId);

        return GetQuestionMessage(chatId, session.CurrentQuestionNumber, kafkaMessageId);
    }

    /// <inheritdoc />
    public BotMessage? CancelTest(string chatId, Guid kafkaMessageId)
    {
        var session = _sessionManager.GetSession(chatId);
        if (session == null)
            return BotMessage.Create(chatId, kafkaMessageId,
                "Тест не начат или завершён", parseMode: null);

        if (_sessionManager.RemoveSession(chatId))
        {
            return BotMessage.Create(chatId, kafkaMessageId,
                $"Тест отменен. Отправьте {BotCommands.StartTestCommand}, чтобы начать заново", parseMode: null);
        }

        return null;
    }

    private BotMessage? GetQuestionMessage(string chatId, int number, Guid kafkaMessageId)
    {
        var question = _questionRepository.GetByNumber(number);
        if (question == null) return null;

        var questionsCount = _questionRepository.GetCount();

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

    private async Task<BotMessage?> FinishTest(UserSession session, Guid kafkaMessageId)
    {
        if (!_sessionManager.RemoveSession(session.ChatId)) return null;

        var result = new TestResult
        {
            ChatId = session.ChatId,
            FinishedAt = DateTime.UtcNow,
            DominanceScore = session.UserAnswers.Count(a => a.SelectedCategories.Contains(DiscType.Dominance)),
            InfluenceScore = session.UserAnswers.Count(a => a.SelectedCategories.Contains(DiscType.Influence)),
            SteadinessScore = session.UserAnswers.Count(a => a.SelectedCategories.Contains(DiscType.Steadiness)),
            ComplianceScore = session.UserAnswers.Count(a => a.SelectedCategories.Contains(DiscType.Compliance))    
        };

        _dbContext.TestResults.Add(result);
        await _dbContext.SaveChangesAsync();

        var message = "Тест завершён! Ваш результат:\n" + MessageFormatter.FormatResult(result);

        return BotMessage.Create(session.ChatId, kafkaMessageId, message, KeyboardBuilder.BuildResultMenuKeyboard());
    }
}