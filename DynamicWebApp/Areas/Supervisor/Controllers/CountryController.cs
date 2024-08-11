namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class CountryController(IRepository<Country, long> repository, IMapper mapper)
    : GenericController<Country, long, CountryListViewModel, CountryDetailViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    (repository, mapper)
{
    public override async Task DataBind()
    {
        ViewBag.CountryEconomicType = EnumExtension.GetSelectList<CountryEconomicType>();

        await Task.CompletedTask;
    }
}
