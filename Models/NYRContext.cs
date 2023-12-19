using Microsoft.EntityFrameworkCore;

namespace nyr_api.Models;

public class NYRContext : DbContext
{
    public NYRContext(DbContextOptions<NYRContext> options)
        : base(options)
    {
    }

    public DbSet<NYRItem> NYRItems { get; set; } = null!;
}