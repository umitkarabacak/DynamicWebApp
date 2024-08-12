namespace DynamicWebApp.Models.Countries;

public class CountryListViewModel : BaseEntityViewModel, IListItemViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ekonomi Türü")]
    public CountryEconomicType? CountryEconomicType { get; set; }
}

public class CountryDetailViewModel : BaseEntityViewModel, IDetailViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }

    [DisplayName("Ekonomi Türü")]
    public CountryEconomicType? CountryEconomicType { get; set; }

    [DisplayName("Bölgeler")]
    public string[] ZoneIds { get; set; }

    [DisplayName("Geçen Paralar")]
    public CurrencyType[] CurrencyTypeIds { get; set; }
}

public class CountryCreateViewModel : BaseEntityViewModel, ICreateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }

    [DisplayName("Ekonomi Türü")]
    public CountryEconomicType CountryEconomicType { get; set; }

    [DisplayName("Bölgeler")]
    public string[] ZoneIds { get; set; }

    [DisplayName("Geçen Paralar")]
    public CurrencyType[] CurrencyTypeIds { get; set; }
}

public class CountryUpdateViewModel : BaseEntityViewModel, IUpdateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }

    [DisplayName("Ekonomi Türü")]
    public CountryEconomicType? CountryEconomicType { get; set; }

    [DisplayName("Bölgeler")]
    public string[] ZoneIds { get; set; }

    [DisplayName("Geçen Paralar")]
    public CurrencyType[] CurrencyTypeIds { get; set; }
}
