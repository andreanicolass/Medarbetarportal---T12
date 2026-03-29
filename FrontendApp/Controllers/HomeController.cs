using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontendApp.Models;

namespace FrontendApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Users");
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Statistik()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
