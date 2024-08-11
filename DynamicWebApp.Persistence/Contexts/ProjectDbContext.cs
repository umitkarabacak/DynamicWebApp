namespace DynamicWebApp.Persistence.Contexts;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options)
    : DbContext(options)
{
    public DbSet<Zone> Zones { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
}
