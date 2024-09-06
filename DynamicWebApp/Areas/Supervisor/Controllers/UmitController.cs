namespace DynamicWebApp.Areas.Supervisior.Controllers;

[Area("Supervisor")]
public class UmitController(IRepository<Umit, Guid> repository, IMapper mapper)
    : GenericController<Umit, Guid, UmitListViewModel, UmitDetailViewModel, UmitCreateViewModel, UmitUpdateViewModel>
    (repository, mapper)
{
}
