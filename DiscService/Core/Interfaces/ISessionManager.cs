using DiscService.Core.Models;

namespace DiscService.Core.Interfaces;

public interface ISessionManager
{
    UserSession? GetSession(string chatId);
    UserSession CreateSession(string chatId);
    bool RemoveSession(string chatId);
    bool HasSession(string chatId);
}