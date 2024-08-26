namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class CityController(IRepository<City, long> repository, IMapper mapper, ProjectDbContext dbContext)
    : GenericController<City, long, CityListViewModel, CityDetailViewModel, CityCreateViewModel, CityUpdateViewModel>
    (repository, mapper: mapper)
{
    [HttpGet]
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

    [HttpGet]
    public override async Task<IActionResult> Detail(long id)
    {
        await DataBind();
        var model = await dbContext.Cities
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));

        if (model == null)
        {
            return NotFound();
        }
        var viewModel = mapper.Map<CityDetailViewModel>(model);
        return View(viewModel);
    }

    public override async Task DataBind()
    {
        var countries = await dbContext.Countries
            .AsNoTracking()
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        ViewBag.CountryId = new SelectList(countries, "Id", "Name");
    }
}
