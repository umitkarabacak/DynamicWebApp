namespace DynamicWebApp.Models.Cities;

public class CityListViewModel : BaseEntityViewModel, IListItemViewModel
{
    [DisplayName("Ülke Adı")]
    public string CountryName { get; set; }

    [DisplayName("Şehir Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }
}

public class CityDetailViewModel : BaseEntityViewModel, IDetailViewModel
{
    [DisplayName("Ülke Adı")]
    public string CountryName { get; set; }

    [DisplayName("Şehir Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Şehir Açıklaması")]
    public string Description { get; set; }
}

public class CityCreateViewModel : BaseEntityViewModel, ICreateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public long CountryId { get; set; }

    [DisplayName("Şehir Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Şehir Açıklaması")]
    public string Description { get; set; }
}

public class CityUpdateViewModel : BaseEntityViewModel, IUpdateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public long CountryListId { get; set; }

    [DisplayName("Şehir Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Şehir Açıklaması")]
    public string Description { get; set; }
}
