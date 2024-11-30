using Microsoft.AspNetCore.Mvc;
using OopProject.Controllers;
using OopProject.Models;
using OopProject.Services;
using System.Security.Claims;

public class UserController : UserHeaderController
{
    private readonly IRequestRepository _requestRepository;

    public UserController(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    // Action to display user requests
    public async Task<IActionResult> RequestBooks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            TempData["ErrorMessage"] = "You must be logged in to view your requests.";
            return RedirectToAction("Login", "Auth");
        }

        // Fetch all requests with Book and Category details
        var requests = await _requestRepository.GetAllRequestsWithDetailsAsync();

        // Filter requests by userId
        var userRequests = requests.Where(r => r.UserId == int.Parse(userId)).ToList();

        return View(userRequests);
    }

public IActionResult UserLogout()
    {
        return RedirectToAction("Logout", "Auth", new { role = "User" });
    }
}
