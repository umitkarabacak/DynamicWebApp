namespace DynamicWebApp.Extensions;

public static class HtmlHelperExtension
{
    public static string IsActive(this IHtmlHelper html, string areas = "", string controllers = "", string actions = "", string cssClass = "active")
    {
        ViewContext viewContext = html.ViewContext;
        string currentArea = viewContext.RouteData.Values["area"]?.ToString() ?? "";
        string currentController = viewContext.RouteData.Values["controller"].ToString();
        string currentAction = viewContext.RouteData.Values["action"].ToString();

        if (string.IsNullOrWhiteSpace(areas))
            areas = currentArea;

        if (string.IsNullOrWhiteSpace(controllers))
            controllers = currentController;

        if (string.IsNullOrWhiteSpace(actions))
            actions = currentAction;

        var acceptedAreas = areas.Trim().Split(',');
        var acceptedControllers = controllers.Trim().Split(',');
        var acceptedActions = actions.Trim().Split(',');

        return acceptedAreas.Contains(currentArea) && acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction) ?
            cssClass : string.Empty;
    }

    public static bool IsParentActive(this IHtmlHelper html, string areas = "", string controllers = "", string actions = "")
    {
        return html.IsActive(areas, controllers, actions) == "active";
    }

    public static object GetIdPropertyValue(this IHtmlHelper html, object obj)
    {
        return html.GetPropertyValue(obj, "Id");
    }

    public static object GetPropertyValue(this IHtmlHelper html, object obj, string propertyName)
    {
        return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null) ?? string.Empty;
    }

    public static string GetDisplayName(this IHtmlHelper html, PropertyInfo property)
    {
        var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), true)
            .Cast<DisplayNameAttribute>()
            .FirstOrDefault()?.DisplayName
            ?? property.GetCustomAttributes(typeof(DisplayAttribute), true)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name
            ?? property.Name;

        return displayName;
    }
}