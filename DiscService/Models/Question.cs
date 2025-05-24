namespace DiscService.Models;

public class Question
{
    public int Number { get; set; }
    public string Text { get; set; }
    public List<AnswerOption> Answers { get; set; }
    
    public Question(int number, string text, List<AnswerOption> answers)
    {
        Number = number;
        Text = text;
        Answers = answers;
    }
}