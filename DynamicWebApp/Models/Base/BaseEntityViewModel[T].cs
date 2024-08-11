namespace DynamicWebApp.Models.Base;

public abstract class BaseEntityViewModel<T>
{
    [DisplayName("Id")]
    public T Id { get; set; }

    public bool IsDeleted { get; set; }
}
