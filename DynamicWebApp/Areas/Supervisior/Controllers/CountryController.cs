namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class CountryController(ProjectDbContext dbContext) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var countries = await dbContext.Countries
            .ToListAsync();

        return Ok(countries);
    }
}
