﻿namespace DynamicWebApp.Models.Country;

public class CountryListViewModel : BaseEntityViewModel, IListItemViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }
}

public class CountryDetailViewModel : BaseEntityViewModel, IDetailViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }
}

public class CountryCreateViewModel : BaseEntityViewModel, ICreateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }
}

public class CountryUpdateViewModel : BaseEntityViewModel, IUpdateViewModel
{
    [DisplayName("Ülke Adı")]
    [Required(ErrorMessage = "{0} zorunlu alandır")]
    public string Name { get; set; }

    [DisplayName("Ülke Açıklaması")]
    public string Description { get; set; }
}
