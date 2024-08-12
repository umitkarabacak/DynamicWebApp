namespace DynamicWebApp.MapperProfiles;

public class DynamicProfile : Profile
{
    public DynamicProfile()
    {
        // Get all assemblies referenced by the current assembly
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Find all types derived from BaseEntity<T>
        var entityTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.BaseType != null &&
                        (
                            (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                            || t.BaseType == typeof(BaseEntity)
                        ))
            .ToList();

        // Find all types derived from BaseEntityViewModel<T>
        var modelTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.BaseType != null &&
                        (
                            (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityViewModel<>))
                            || t.BaseType == typeof(BaseEntityViewModel)
                        ))
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
}
