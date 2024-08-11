using DynamicWebApp.Areas.Controllers;
using DynamicWebApp.Models.Countries;

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
[Area("Supervisor")]
public class GenericCountryController(IRepository<Country, long> repository, IMapper mapper) : GenericController<Country, long, CountryListViewModel, CountryDetailViewModel, CountryCreateViewModel, CountryUpdateViewModel>(repository, mapper)
{
}
