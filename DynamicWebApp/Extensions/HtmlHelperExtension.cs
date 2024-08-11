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
}