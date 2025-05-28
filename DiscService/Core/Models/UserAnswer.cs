namespace DiscService.Core.Models;

public class UserAnswer
{
    public int QuestionNumber { get; set; }
    public string SelectedOption { get; set; }
    public HashSet<DiscType> SelectedCategories { get; set; }

    public UserAnswer(int questionNumber, string selectedOption, params DiscType[] selectedCategories)
    {
        QuestionNumber = questionNumber;
        SelectedOption = selectedOption;
        SelectedCategories = new HashSet<DiscType>(selectedCategories);
    }
}