using System.Collections.Concurrent;
using DiscService.Core.Interfaces;
using DiscService.Core.Models;

namespace DiscService.Core.Services;

/// <summary>
/// Реализация менеджера сессий <see cref="ISessionManager"/> пользователей в памяти.
/// </summary>
public class SessionManager : ISessionManager
{
    private readonly ConcurrentDictionary<string, UserSession> _sessions = new();

    /// <inheritdoc />
    public UserSession? GetSession(string chatId)
    {
        _sessions.TryGetValue(chatId, out var session);
        return session;
    }
    
    /// <inheritdoc />
    public UserSession CreateSession(string chatId)
    {
        var session = new UserSession(chatId);
        _sessions[chatId] = session;
        return session;
    }

    /// <inheritdoc />
    public bool RemoveSession(string chatId)
    {
        return _sessions.TryRemove(chatId, out _);
    }

    /// <inheritdoc />
    public bool HasSession(string chatId)
    {
        return _sessions.TryGetValue(chatId, out _);
    }
}