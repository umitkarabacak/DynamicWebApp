namespace DynamicWebApp.Persistence;

public static class DI
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemoryDatabase = Convert.ToBoolean(configuration.GetSection("UseInMemoryDatabase").Value);

        services.AddDbContext<ProjectDbContext>(optionsBuilder =>
        {
            if (useInMemoryDatabase)
                optionsBuilder.UseInMemoryDatabase(nameof(ProjectDbContext));
            else
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(nameof(ProjectDbContext)));

            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw());
        });
    }
}
