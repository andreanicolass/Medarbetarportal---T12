using FrontendApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FrontendApp.Controllers;

public class ApplicationsController : Controller
{
    private readonly HttpClient _httpClient;

    public ApplicationsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApplicationsApi");
    }

    // GET: /applications
    public async Task<IActionResult> Index()
    {
        var applications = await _httpClient.GetFromJsonAsync<List<ApplicationViewModel>>("api/applications");
        return View(applications);
    }

    // GET: /applications/create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /applications/create
    [HttpPost]
    public async Task<IActionResult> Create(ApplicationViewModel model)
    {
        await _httpClient.PostAsJsonAsync("api/applications", model);
        return RedirectToAction(nameof(Index));
    }

    // GET: /applications/edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var application = await _httpClient.GetFromJsonAsync<ApplicationViewModel>($"api/applications/{id}");
        return View(application);
    }

    // POST: /applications/edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(int id, ApplicationViewModel model)
    {
        await _httpClient.PutAsJsonAsync($"api/applications/{id}", model);
        return RedirectToAction(nameof(Index));
    }

    // GET: /applications/delete/{id}
    public async Task<IActionResult> Delete(int id)
    {
        var application = await _httpClient.GetFromJsonAsync<ApplicationViewModel>($"api/applications/{id}");
        return View(application);
    }

    // POST: /applications/delete/{id}
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _httpClient.DeleteAsync($"api/applications/{id}");
        return RedirectToAction(nameof(Index));
    }
}