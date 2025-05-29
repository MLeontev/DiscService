using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс репозитория для получения вопросов DISC-теста.
/// </summary>
public interface IQuestionRepository
{
    /// <summary>
    /// Возвращает вопрос по его номеру.
    /// </summary>
    /// <param name="number">Номер вопроса</param>
    /// <returns>Вопрос <see cref="Question"/>, если вопрос найден; иначе <c>null</c>.</returns>
    Question? GetByNumber(int number);
    
    /// <summary>
    /// Возвращает список всех вопросов DISC-теста.
    /// </summary>
    /// <returns>Список всех вопросов <see cref="Question"/>.</returns>
    List<Question> GetAll();
    
    /// <summary>
    /// Возвращает общее количество вопросов в тесте.
    /// </summary>
    /// <returns>Количество вопросов.</returns>
    int GetCount();
}