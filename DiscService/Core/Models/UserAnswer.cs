namespace DiscService.Core.Models;

/// <summary>
/// Представляет ответ пользователя на вопрос DISC-теста.
/// </summary>
public class UserAnswer
{
    /// <summary>
    /// Номер вопроса, на который дал ответ пользователь.
    /// </summary>
    public int QuestionNumber { get; set; }
    
    /// <summary>
    /// Обозначение выбранного варианта ответа.
    /// </summary>
    public string SelectedOption { get; set; }
    
    /// <summary>
    /// Набор типов DISC, связанных с выбранным вариантом ответа.
    /// Может содержать один или несколько типов.
    /// </summary>
    public HashSet<DiscType> SelectedCategories { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserAnswer"/>.
    /// </summary>
    /// <param name="questionNumber">Номер вопроса.</param>
    /// <param name="selectedOption">Выбранный вариант ответа.</param>
    /// <param name="selectedCategories">Один или несколько типов DISC, связанных с этим вариантом.</param>
    public UserAnswer(int questionNumber, string selectedOption, params DiscType[] selectedCategories)
    {
        QuestionNumber = questionNumber;
        SelectedOption = selectedOption;
        SelectedCategories = new HashSet<DiscType>(selectedCategories);
    }
}