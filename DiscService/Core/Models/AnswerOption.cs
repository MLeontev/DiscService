namespace DiscService.Core.Models;

/// <summary>
/// Представляет вариант ответа на вопрос DISC-теста.
/// </summary>
public class AnswerOption
{
    /// <summary>
    /// Метка варианта ответа.
    /// </summary>
    public string Label { get; set; }
    
    /// <summary>
    /// Текст варианта ответа.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Набор типов DISC, с которыми связан данный вариант ответа.
    /// Может содержать один или несколько типов.
    /// </summary>
    public HashSet<DiscType> DiscTypes { get; set; }
    
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AnswerOption"/>.
    /// </summary>
    /// <param name="label">Метка варианта ответа.</param>
    /// <param name="text">Текст варианта ответа.</param>
    /// <param name="discTypes">Один или несколько типов DISC, связанных с этим вариантом.</param>
    public AnswerOption(string label, string text, params DiscType[] discTypes)
    {
        Label = label;
        Text = text;
        DiscTypes = new HashSet<DiscType>(discTypes);
    }
}