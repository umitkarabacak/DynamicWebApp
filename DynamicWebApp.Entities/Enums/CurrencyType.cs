namespace DynamicWebApp.Entities.Enums;

public enum CurrencyType
{
    [Description("Tanımsız")]
    Unkown = 0,
    [Description("Turkish Lira")]
    TL = 100,
    [Description("Dolar")]
    Dolar = 200,
    [Description("Euro")]
    Euro = 300,
    [Description("Pound")]
    Pound = 400,
}
