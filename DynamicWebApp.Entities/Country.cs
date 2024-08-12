namespace DynamicWebApp.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; }
    public CountryEconomicType CountryEconomicType { get; set; }
    public string Description { get; set; }
    public string ZoneIds { get; set; }
}
