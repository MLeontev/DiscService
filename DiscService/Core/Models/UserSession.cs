namespace DiscService.Core.Models;

/// <summary>
/// Представляет сессию пользователя во время прохождения DISC-теста.
/// </summary>
public class UserSession
{
    /// <summary>
    /// Id чата Telegram, к которому привязана сессия.
    /// </summary>
    public string ChatId { get; set; }
    
    /// <summary>
    /// Номер текущего вопроса.
    /// </summary>
    public int CurrentQuestionNumber { get; set; }
    
    /// <summary>
    /// Список ответов пользователя на вопросы теста.
    /// </summary>
    public List<UserAnswer> UserAnswers { get; set; } = [];

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserSession"/>.
    /// </summary>
    /// <param name="chatId">Id чата Telegram, к которому привязана сессия.</param>
    public UserSession(string chatId)
    {
        ChatId = chatId;
    }
}