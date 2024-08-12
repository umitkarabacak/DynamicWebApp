namespace DynamicWebApp.Entities.Enums;

public enum CountryEconomicType
{
    [Description("Tanımsız")]
    Unkown = 0,
    [Description("Gelişmemiş")]
    LowDeveloped = 100,
    [Description("Az Gelişmiş")]
    UnderDeveloped = 500,
    [Description("Gelişmiş")]
    Developed = 1000,
}
