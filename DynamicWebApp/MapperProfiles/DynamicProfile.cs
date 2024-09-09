namespace DynamicWebApp.MapperProfiles;

public class DynamicProfile : Profile
{
    public DynamicProfile()
    {
        var modelBaseEntityAssembly = typeof(BaseEntity<>).Assembly;
        var modelBaseEntityViewModelAssembly = typeof(BaseEntityViewModel<>).Assembly;
        var assemblies = new[] { modelBaseEntityAssembly, modelBaseEntityViewModelAssembly };

        // Find all types derived from BaseEntity<T>
        var entityTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.BaseType != null &&
                        (
                            (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                            || t.BaseType == typeof(BaseEntity)
                        ))
            .Distinct()
            .ToList();

        // Find all types derived from BaseEntityViewModel<T>
        var modelTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.BaseType != null &&
                        (
                            (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityViewModel<>))
                            || t.BaseType == typeof(BaseEntityViewModel)
                        ))
            .Distinct()
            .ToList();

        foreach (var entityType in entityTypes)
        {
            var matchingModels = modelTypes.Where(m => m.Name.StartsWith(entityType.Name)).ToList();

            foreach (var modelType in matchingModels)
            {
                var map = CreateMap(entityType, modelType);

                // Handle properties ending with "Ids"
                var idsProperties = modelType.GetProperties()
                    .Where(p => p.Name.EndsWith("Ids") &&
                               (p.PropertyType == typeof(string[]) ||
                                p.PropertyType == typeof(int[]) ||
                                p.PropertyType == typeof(long[]) ||
                                p.PropertyType == typeof(Guid[])))
                    .Distinct()
                    .ToList();

                foreach (var property in idsProperties)
                {
                    var entityProp = entityType.GetProperty(property.Name);
                    if (entityProp != null)
                    {
                        if (property.PropertyType == typeof(string[]))
                        {
                            map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src) ?? string.Empty)));
                            map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((string[])property.GetValue(src) ?? Array.Empty<string>())));
                        }
                        //else if (property.PropertyType == typeof(int[]))
                        //{
                        //    map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src) ?? string.Empty).Select(int.Parse).ToArray()));
                        //    map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((int[])property.GetValue(src) ?? Array.Empty<int>())));
                        //}
                        //else if (property.PropertyType == typeof(long[]))
                        //{
                        //    map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src) ?? string.Empty).Select(long.Parse).ToArray()));
                        //    map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((long[])property.GetValue(src) ?? Array.Empty<long>())));
                        //}
                        //else if (property.PropertyType == typeof(Guid[]))
                        //{
                        //    map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src) ?? string.Empty).Select(Guid.Parse).ToArray()));
                        //    map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((Guid[])property.GetValue(src) ?? Array.Empty<Guid>())));
                        //}
                    }
                }

                // Handle enum arrays
                var enumProperties = modelType.GetProperties()
                    .Where(p => p.PropertyType.IsArray && p.PropertyType.GetElementType().IsEnum)
                    .ToList();

                //foreach (var property in enumProperties)
                //{
                //    var entityProp = entityType.GetProperty(property.Name);
                //    if (entityProp != null)
                //    {
                //        var enumType = property.PropertyType.GetElementType();
                //        map.ForMember(property.Name, opts => opts.MapFrom(src => convertToEnumArray((string)entityProp.GetValue(src) ?? string.Empty, enumType)));
                //        map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertEnumArrayToString((Array)property.GetValue(src) ?? Array.Empty<Enum>())));
                //    }
                //}

                // If there are no special mappings for Ids or Enum, apply ReverseMap directly
                if (idsProperties.Count == 0 && enumProperties.Count == 0)
                {
                    map.ReverseMap();
                }
            }
        }
    }

    static string[] convertToArray(string ids)
    {
        return string.IsNullOrWhiteSpace(ids) ? Array.Empty<string>() : ids.Split(',', StringSplitOptions.RemoveEmptyEntries);
    }

    static string convertToString(Array ids)
    {
        return ids == null || ids.Length == 0 ? string.Empty : string.Join(",", ids.Cast<object>());
    }

    //private object[] convertToEnumArray(string ids, Type enumType)
    //{
    //    return convertToArray(ids).Select(x => Enum.Parse(enumType, x)).ToArray();
    //}

    //private string convertEnumArrayToString(Array enumArray)
    //{
    //    return enumArray == null || enumArray.Length == 0 ? string.Empty : string.Join(",", enumArray.Cast<Enum>().Select(e => e.ToString()));
    //}
}
