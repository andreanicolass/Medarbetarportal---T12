namespace FrontendApp.Controllers;

using Microsoft.AspNetCore.Mvc;

public class UsersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Admin()
    {
        return View();
    }
}