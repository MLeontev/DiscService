namespace DiscService.Models;

public class UserAnswer
{
    public int QuestionNumber { get; set; }
    public string SelectedOption { get; set; }
    public DiscType SelectedCategory { get; set; }

    public UserAnswer(int questionNumber, string selectedOption, DiscType selectedCategory)
    {
        QuestionNumber = questionNumber;
        SelectedOption = selectedOption;
        SelectedCategory = selectedCategory;
    }
}