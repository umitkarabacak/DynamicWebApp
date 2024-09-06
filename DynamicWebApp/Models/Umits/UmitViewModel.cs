namespace DynamicWebApp.Models.Umits;

public class UmitListViewModel : BaseEntityViewModel<Guid>, IListItemViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }
}

public class UmitDetailViewModel : BaseEntityViewModel<Guid>, IDetailViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}

public class UmitCreateViewModel : BaseEntityViewModel<Guid>, ICreateViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}

public class UmitUpdateViewModel : BaseEntityViewModel<Guid>, IUpdateViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}
