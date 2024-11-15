using Microsoft.AspNetCore.Mvc;
using OopProject.Models;

namespace OopProject.Controllers
{
    public class BooksController : HeaderController
    {
        public IActionResult PerCategory()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to the login page if the user is not authenticated
                return RedirectToAction("UserLogin", "Auth");
            }

            return View(); // Continue to PerCategory page if authenticated
        }
        public IActionResult BookDetails()
        {
            return View();
        }

        // POST: RequestBooks (when the user requests a book)
        [HttpPost]
        public IActionResult RequestBooks(string Name, string Email, string PhoneNum)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, redirect to the login page
                return RedirectToAction("UserLogin", "Auth");
            }

            // Logic to handle the request, such as saving the request or sending a confirmation email
            // Example: Save the request to the database

            TempData["SuccessMessage"] = "Your request has been submitted successfully!"; // Success message for the user

            return RedirectToAction("Success"); // Redirect to the success page after request is processed
        }

        // Success page after the request has been successfully made
        public IActionResult Success()
        {
            return View(); // Display a success page
        }
    }
}
