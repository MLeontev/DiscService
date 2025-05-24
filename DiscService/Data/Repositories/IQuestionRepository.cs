using DiscService.Models;

namespace DiscService.Data.Repositories;

public interface IQuestionRepository
{
    Question? GetByNumber(int number);
    List<Question> GetAll();
}