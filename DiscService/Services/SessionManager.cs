using System.Collections.Concurrent;
using DiscService.Models;

namespace DiscService.Services;

public class SessionManager
{
    private readonly ConcurrentDictionary<string, UserSession> _sessions = new();

    public UserSession? GetSession(string chatId)
    {
        _sessions.TryGetValue(chatId, out var session);
        return session;
    }

    public UserSession CreateSession(string chatId)
    {
        var session = new UserSession(chatId);
        _sessions[chatId] = session;
        return session;
    }

    public bool RemoveSession(string chatId)
    {
        return _sessions.TryRemove(chatId, out _);
    }

    public bool HasSession(string chatId)
    {
        return _sessions.TryGetValue(chatId, out _);
    }
}