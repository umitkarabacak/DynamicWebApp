namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class ZoneController(IRepository<Zone, long> repository, IMapper mapper)
    : GenericController<Zone, long, ZoneListViewModel, ZoneDetailViewModel, ZoneCreateViewModel, ZoneUpdateViewModel>
    (repository, mapper)
{
}
