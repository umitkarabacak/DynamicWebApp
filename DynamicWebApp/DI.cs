namespace DynamicWebApp;

public static class DI
{
    public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMvc()
            .AddRazorRuntimeCompilation();
    }
}
