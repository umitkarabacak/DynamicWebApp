using System.Reflection;

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
}