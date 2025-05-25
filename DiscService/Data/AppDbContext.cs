using DiscService.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TestResult> TestResults { get; set; }
}