namespace DynamicWebApp.Extensions;

public static class IdentityResultExtension
{
    public static List<IdentityError> GetErrorList(this IdentityResult identityResult)
        => identityResult.Errors.DistinctBy(e => e.Code).ToList();
}
