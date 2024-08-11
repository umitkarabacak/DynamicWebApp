namespace DynamicWebApp.Configs;

public class DatabaseInitializer(IServiceProvider serviceProvider) : IHostedService
{
    private ProjectDbContext dbContext;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();

        await seedData(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    private async Task seedData(CancellationToken cancellationToken)
    {
        var countries = await dbContext.Countries
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (countries.Count == 0)
        {
            countries =
            [
                new Country { Id = 1, Name= "Türkiye" }
            ];

            await dbContext.Countries.AddRangeAsync(countries, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
