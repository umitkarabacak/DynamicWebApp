namespace DynamicWebApp.Entities;

public class City : BaseEntity
{
    public Country Country { get; set; }
    public long CountryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
