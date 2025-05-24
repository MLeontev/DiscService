using DiscService.Models;

namespace DiscService.Data.Repositories;

public class InMemoryQuestionRepository : IQuestionRepository
{
    public Question? GetByNumber(int number)
    {
        throw new NotImplementedException();
    }

    public List<Question> GetAll()
    {
        throw new NotImplementedException();
    }
}