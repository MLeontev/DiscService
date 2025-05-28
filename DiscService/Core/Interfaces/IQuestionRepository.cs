using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

public interface IQuestionRepository
{
    Question? GetByNumber(int number);
    List<Question> GetAll();
    int GetCount();
}