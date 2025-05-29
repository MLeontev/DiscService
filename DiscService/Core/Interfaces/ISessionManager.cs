using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

/// <summary>
/// Интерфейс для управления сессиями пользователей во время прохождения DISC-теста.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Получает сессию пользователя по идентификатору чата.
    /// </summary>
    /// <param name="chatId">Id чата Telegram.</param>
    /// <returns>Объект <see cref="UserSession"/>, если сессия существует; иначе <c>null</c>.</returns>
    UserSession? GetSession(string chatId);
    
    /// <summary>
    /// Создаёт новую сессию пользователя.
    /// </summary>
    /// <param name="chatId">Id чата Telegram.</param>
    /// <returns>Созданная <see cref="UserSession"/>.</returns>
    UserSession CreateSession(string chatId);
    
    /// <summary>
    /// Удаляет сессию пользователя.
    /// </summary>
    /// <param name="chatId">Id чата Telegram.</param>
    /// <returns><c>true</c>, если сессия была удалена; иначе <c>false</c>.</returns>
    bool RemoveSession(string chatId);
    
    /// <summary>
    /// Проверяет, существует ли сессия пользователя.
    /// </summary>
    /// <param name="chatId">Id чата Telegram.</param>
    /// <returns><c>true</c>, если сессия существует; иначе <c>false</c>.</returns>
    bool HasSession(string chatId);
}