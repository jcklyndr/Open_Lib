using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    [Authorize(AuthenticationSchemes = "UserScheme")] // Applies to all actions in the controller
    public class BooksController : UserHeaderController
    {
        public IActionResult PerCategory()
        {
            return View(); // Only accessible if authenticated
        }

        public IActionResult BookDetails()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RequestBooks(string Name, string Email, string PhoneNum)
        {
            TempData["SuccessMessage"] = "Your request has been submitted successfully!";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
