namespace DynamicWebApp.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        logger.LogInformation(nameof(Index));
        return View();
    }
}
