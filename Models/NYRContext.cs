using Microsoft.EntityFrameworkCore;

namespace nyr_api.Models;

public class NYRContext : DbContext
{
    public NYRContext(DbContextOptions<NYRContext> options)
        : base(options)
    {
    }

    public DbSet<NYREvent> NYREvents { get; set; } = null!;
}