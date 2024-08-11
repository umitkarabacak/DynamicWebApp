using DynamicWebApp.Areas.Controllers;
using DynamicWebApp.Models.Cities;
using DynamicWebApp.Models.Countries;

namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class CountryController(IRepository<Country, long> repository, IMapper mapper) : GenericController<Country, long, CountryListViewModel, CountryDetailViewModel, CountryCreateViewModel, CountryUpdateViewModel>(repository, mapper)
{
}

[Area("Supervisor")]
public class CityController(IRepository<City, long> repository, IMapper mapper, ProjectDbContext dbContext) : GenericController<City, long, CityListViewModel, CountryDetailViewModel, CountryCreateViewModel, CountryUpdateViewModel>(repository, mapper)
{
    public override async Task<IActionResult> Index()
    {
        await DataBind();
        var models = await dbContext.Cities
            .AsNoTracking()
            .Include(c => c.Country)
            .ToListAsync();
        var viewModels = mapper.Map<List<CityListViewModel>>(models);
        return View(viewModels);
    }
}
