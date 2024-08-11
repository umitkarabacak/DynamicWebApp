using DynamicWebApp.Areas.Controllers;

namespace DynamicWebApp;

public static class DI
{
    public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddMvc()
            .AddRazorRuntimeCompilation();

        services.AddPersistence(configuration);

        services.AddHostedService<DatabaseInitializer>();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
    }
}
