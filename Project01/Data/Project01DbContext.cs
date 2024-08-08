using Microsoft.EntityFrameworkCore;

namespace Project01.Data;
public class Project01DbContext : DbContext
{
    public Project01DbContext(DbContextOptions<Project01DbContext> options)
        : base(options) { }

    public DbSet<SearchQuery> SearchQueries { get; set; }
}