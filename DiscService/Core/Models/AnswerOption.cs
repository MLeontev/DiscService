namespace DiscService.Core.Models;

public class AnswerOption
{
    public string Label { get; set; }
    public string Text { get; set; }
    public DiscType DiscType { get; set; }
    
    public AnswerOption(string label, string text, DiscType discType)
    {
        Label = label;
        Text = text;
        DiscType = discType;
    }
}