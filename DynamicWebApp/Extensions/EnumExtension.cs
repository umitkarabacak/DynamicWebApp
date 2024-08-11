namespace DynamicWebApp.Extensions;

public static class EnumExtension
{
    public static string GetEnumDescription(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Length != 0)
        {
            return attributes.FirstOrDefault()?.Description;
        }

        return value.ToString();
    }

    public static SelectList GetSelectList<TEnum>() where TEnum : Enum
    {
        var enumValues = Enum.GetValues(typeof(TEnum))
            .Cast<Enum>()
            .Select(e => new SelectListItem
            {
                Value = Convert.ToInt32(e).ToString(),
                Text = e.GetEnumDescription() ?? e.ToString()
            }).ToList();

        return new SelectList(enumValues, "Value", "Text");
    }
}