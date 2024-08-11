namespace DynamicWebApp.Models.Zones;

public class ZoneListViewModel : BaseEntityViewModel, IListItemViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }
}

public class ZoneDetailViewModel : BaseEntityViewModel, IDetailViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}

public class ZoneCreateViewModel : BaseEntityViewModel, ICreateViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}

public class ZoneUpdateViewModel : BaseEntityViewModel, IUpdateViewModel
{
    [DisplayName("Bölge Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Bölge Açıklaması")]
    public string Description { get; set; }
}
