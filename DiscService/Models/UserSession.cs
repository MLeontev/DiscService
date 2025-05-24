namespace DiscService.Models;

public class UserSession
{
    public string ChatId { get; set; }
    public int CurrentQuestionNumber { get; set; } = 0;
    public List<UserAnswer> UserAnswers { get; set; } = [];

    public UserSession(string chatId)
    {
        ChatId = chatId;
    }
}