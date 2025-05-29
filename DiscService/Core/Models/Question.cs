namespace DiscService.Core.Models;

/// <summary>
/// Представляет вопрос DISC-теста.
/// </summary>
public class Question
{
    /// <summary>
    /// Номер вопроса в тесте.
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// Текст вопроса.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Список вариантов ответов на вопрос.
    /// </summary>
    public List<AnswerOption> Answers { get; set; }
    
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Question"/>.
    /// </summary>
    /// <param name="number">Номер вопроса.</param>
    /// <param name="text">Текст вопроса.</param>
    /// <param name="answers">Список вариантов ответов.</param>
    public Question(int number, string text, List<AnswerOption> answers)
    {
        Number = number;
        Text = text;
        Answers = answers;
    }
}