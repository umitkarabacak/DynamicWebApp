namespace DynamicWebApp.MapperProfiles;

public class DynamicProfile : Profile
{
    public DynamicProfile()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // BaseEntity<T> sınıfından türeyen tüm türleri bulun
        var entityTypes = assembly.GetTypes()
            .Where(t => t.BaseType != null &&
                        (
                            t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>)
                            || t.BaseType == typeof(BaseEntity)
                        )
                    )
            .ToList();

        // BaseEntityViewModel<T> sınıfından türeyen tüm türleri bulun
        var modelTypes = assembly.GetTypes()
            .Where(t => t.BaseType != null &&
                        (
                            t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityViewModel<>)
                            || t.BaseType == typeof(BaseEntityViewModel)
                        )
                    )
            .ToList();
        var createMapMethod = typeof(Profile).GetMethod("CreateMap", new Type[] { });

        foreach (var entityType in entityTypes)
        {
            // Her entity türü için, aynı isimle başlayan model türlerini bulun
            var matchingModels = modelTypes.Where(m => m.Name.StartsWith(entityType.Name)).ToList();

            foreach (var modelType in matchingModels)
            {
                // Her eşleşen tür için map oluşturun
                var genericCreateMapMethod = createMapMethod.MakeGenericMethod(entityType, modelType);
                var map = genericCreateMapMethod.Invoke(this, null);

                // ReverseMap'i çağır
                var reverseMapMethod = map.GetType().GetMethod("ReverseMap");
                reverseMapMethod.Invoke(map, null);
            }
        }
    }
}
