namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class CountryController(IRepository<Country, long> repository, IMapper mapper, ProjectDbContext dbContext)
    : GenericController<Country, long, CountryListViewModel, CountryDetailViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    (repository, mapper)
{
    public override async Task DataBind()
    {
        ViewBag.CountryEconomicType = EnumExtension.GetSelectList<CountryEconomicType>();
        ViewBag.CurrencyTypeIds = EnumExtension.GetSelectList<CurrencyType>();

        var zones = await dbContext.Zones.AsNoTracking().Select(z => new { z.Id, z.Name }).ToListAsync();
        ViewBag.ZoneIds = new SelectList(zones, "Id", "Name");
    }
}
