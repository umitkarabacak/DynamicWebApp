namespace DynamicWebApp.Extensions;

public static class ModelStateExtension
{
    public static List<ModelError> GetModelErrors(this ModelStateDictionary modelStateDictionary)
        => modelStateDictionary.Values.SelectMany(i => i.Errors).ToList();

    public static void BindModelErrors(this ModelStateDictionary modelStateDictionary)
    {
        modelStateDictionary.Values.SelectMany(i => i.Errors).ToList().ForEach(error =>
        {
            modelStateDictionary.AddModelError(error?.ErrorMessage, error?.Exception?.ToString() ?? error.ErrorMessage);
        });
    }

    public static void BindModelErrors(this ModelStateDictionary modelStateDictionary, List<IdentityError> identityErrors)
    {
        identityErrors.ForEach(error =>
        {
            modelStateDictionary.AddModelError(string.Empty, error?.Description ?? string.Empty);
        });
    }
}
