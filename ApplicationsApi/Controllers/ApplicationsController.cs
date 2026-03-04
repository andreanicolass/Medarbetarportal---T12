using Microsoft.AspNetCore.Mvc;
using ApplicationsApi.Models;

namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private static List<BenefitApplication> _applications = new();

    // GET api/applications
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_applications);
    }

    // POST api/applications
    [HttpPost]
    public IActionResult Create(BenefitApplication application)
    {
        application.Id = _applications.Count + 1;
        application.CreatedDate = DateTime.Now;
        application.Status = "Pending";
        _applications.Add(application);
        return CreatedAtAction(nameof(GetAll), application);
    }
}