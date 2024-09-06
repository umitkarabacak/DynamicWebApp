namespace DynamicWebApp.MapperProfiles;

public class DynamicProfile : Profile
{
    public DynamicProfile()
    {
        // BaseEntity<> ve BaseEntityViewModel<> türlerinden birinin assembly'sini alın
        var modelBaseEntityAssembly = typeof(BaseEntity<>).Assembly;
        var modelBaseEntityViewModelAssembly = typeof(BaseEntityViewModel<>).Assembly;

        var assemblies = new[] { modelBaseEntityAssembly, modelBaseEntityViewModelAssembly };

        //// Get all assemblies referenced by the current assembly
        ////var assemblies = AppDomain.CurrentDomain.GetAssemblies();

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
            // Her entity türü için, aynı isimle başlayan model türlerini bulun
            var matchingModels = modelTypes.Where(m => m.Name.StartsWith(entityType.Name)).ToList();

            foreach (var modelType in matchingModels)
            {
                // Her eşleşen tür için map oluşturun
                var map = CreateMap(entityType, modelType);

                // Sonu "Ids" ile biten alanlar için özel map işlemleri ekleyin
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
                            map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src))));
                            map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((string[])property.GetValue(src))));
                        }
                        else if (property.PropertyType == typeof(int[]))
                        {
                            map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src)).Select(int.Parse).ToArray()));
                            map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((int[])property.GetValue(src))));
                        }
                        else if (property.PropertyType == typeof(long[]))
                        {
                            map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src)).Select(long.Parse).ToArray()));
                            map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((long[])property.GetValue(src))));
                        }
                        else if (property.PropertyType == typeof(Guid[]))
                        {
                            map.ForMember(property.Name, opts => opts.MapFrom(src => convertToArray((string)entityProp.GetValue(src)).Select(Guid.Parse).ToArray()));
                            map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertToString((Guid[])property.GetValue(src))));
                        }
                    }
                }

                // Enum dizileri için map işlemleri ekleyin
                var enumProperties = modelType.GetProperties()
                    .Where(p => p.PropertyType.IsArray && p.PropertyType.GetElementType().IsEnum)
                    .ToList();

                foreach (var property in enumProperties)
                {
                    var entityProp = entityType.GetProperty(property.Name);
                    if (entityProp != null)
                    {
                        var enumType = property.PropertyType.GetElementType();
                        map.ForMember(property.Name, opts => opts.MapFrom(src => convertToEnumArray((string)entityProp.GetValue(src), enumType)));
                        map.ReverseMap().ForMember(property.Name, opts => opts.MapFrom(src => convertEnumArrayToString((Array)property.GetValue(src))));
                    }
                }

                // Enum Array yada Ids ile bitmeyenler için ReverseMap'i çağır
                if (idsProperties.Count == 0 && enumProperties.Count == 0)
                {
                    var reverseMapMethod = map.GetType().GetMethod("ReverseMap");
                    reverseMapMethod.Invoke(map, null);
                }
            }
        }
    }

    static string[] convertToArray(string ids)
    {
        return string.IsNullOrWhiteSpace(ids) ? [] : ids.Split(',');
    }

    static string convertToString(Array ids)
    {
        return ids == null || ids.Length == 0 ? string.Empty : string.Join(",", ids.Cast<object>());
    }

    private object[] convertToEnumArray(string ids, Type enumType)
    {
        return convertToArray(ids).Select(x => Enum.Parse(enumType, x)).ToArray();
    }

    private string convertEnumArrayToString(Array enumArray)
    {
        return string.Join(",", enumArray.Cast<Enum>().Select(e => e.ToString()));
    }
}
