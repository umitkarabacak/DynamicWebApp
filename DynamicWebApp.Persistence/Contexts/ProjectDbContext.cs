namespace DynamicWebApp.Persistence.Contexts;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options)
    : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
}
