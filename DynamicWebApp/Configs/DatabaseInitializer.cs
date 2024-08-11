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
        await seedDataCountries(cancellationToken);
        await seedDataCities(cancellationToken);
    }

    private async Task seedDataCountries(CancellationToken cancellationToken)
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

    private async Task seedDataCities(CancellationToken cancellationToken)
    {
        var cities = await dbContext.Cities
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (cities.Count == 0)
        {
            cities =
            [
                new City { Id = 1, CountryId = 1, Name= "Adana" },
                new City { Id = 2, CountryId = 1, Name= "Adıyaman" },
                new City { Id = 3, CountryId = 1, Name= "Afyon" },
                new City { Id = 4, CountryId = 1, Name= "Ağrı" },
                new City { Id = 5, CountryId = 1, Name= "Amasya" },
                new City { Id = 6, CountryId = 1, Name= "Ankara" },
            ];

            await dbContext.Cities.AddRangeAsync(cities, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
