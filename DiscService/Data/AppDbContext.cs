using DiscService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscService.Data;

/// <summary>
/// Контекст базы данных для DISC-сервиса.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AppDbContext"/> с указанными опциями.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Набор результатов DISC-тестов.
    /// </summary>
    public DbSet<TestResult> TestResults { get; set; }
}