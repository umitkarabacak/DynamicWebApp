namespace DynamicWebApp.Extensions;

public static class PrincipalExtensions
{
    public static string GetFullName(this IPrincipal user)
    {
        if (!user.Identity.IsAuthenticated)
            return string.Empty;

        ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;

        var fullNameClaim = claimsIdentity.Claims.FirstOrDefault(ci => ci.Type.Equals("Fullname"))?.Value ?? string.Empty;

        return fullNameClaim;
    }

    public static string GetProfilePhoto(this IPrincipal user)
    {
        if (!user.Identity.IsAuthenticated)
            return string.Empty;

        ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;

        var profilePhotoClaim = claimsIdentity.Claims.FirstOrDefault(ci => ci.Type.Equals("ProfilePhoto"))?.Value ?? string.Empty;

        return profilePhotoClaim;
    }

    public static string GetCurrentUserId(this IPrincipal user)
    {
        if (!user.Identity.IsAuthenticated)
            return string.Empty;

        var claimsIdentity = (ClaimsIdentity)user.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId2 = claim.Value;
        var userId = claimsIdentity.Claims.FirstOrDefault(ci => ci.Type.Equals(ClaimTypes.NameIdentifier))?.Value ?? string.Empty;

        return userId;
    }

    public static Guid GetCurrentUserGuid(this IPrincipal user)
    {
        if (!user.Identity.IsAuthenticated)
            return Guid.Empty;

        var claimsIdentity = (ClaimsIdentity)user.Identity;
        var userId = claimsIdentity?.Claims?.FirstOrDefault(ci => ci.Type.Equals(ClaimTypes.NameIdentifier))?.Value ?? string.Empty;

        return string.IsNullOrWhiteSpace(userId)
            ? Guid.Empty
            : Guid.Parse(userId);
    }
}
