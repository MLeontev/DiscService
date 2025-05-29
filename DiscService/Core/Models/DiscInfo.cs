namespace DiscService.Core.Models;

/// <summary>
/// Представляет информацию о психотипе DISC.
/// </summary>
public class DiscInfo
{
    /// <summary>
    /// Психотип DISC.
    /// </summary>
    public DiscType DiscType { get; set; }
    
    /// <summary>
    /// Название психотипа.
    /// </summary>
    public string DiscName { get; set; }
    
    /// <summary>
    /// Ключевые слова, характеризующие психотип.
    /// </summary>
    public List<string> Keywords {get; set; }
    
    /// <summary>
    /// Описание психотипа.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DiscInfo"/>.
    /// </summary>
    /// <param name="discType">Тип DISC.</param>
    /// <param name="discName">Имя типа.</param>
    /// <param name="keywords">Ключевые слова.</param>
    /// <param name="description">Описание.</param>
    public DiscInfo(DiscType discType, string discName, List<string> keywords, string description)
    {
        DiscType = discType;
        DiscName = discName;
        Keywords = keywords;
        Description = description;
    }
}