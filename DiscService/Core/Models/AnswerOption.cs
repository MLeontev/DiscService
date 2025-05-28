namespace DiscService.Core.Models;

public class AnswerOption
{
    public string Label { get; set; }
    public string Text { get; set; }
    public HashSet<DiscType> DiscTypes { get; set; }
    
    public AnswerOption(string label, string text, params DiscType[] discTypes)
    {
        Label = label;
        Text = text;
        DiscTypes = new HashSet<DiscType>(discTypes);
    }
}