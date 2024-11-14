using Microsoft.AspNetCore.Mvc;


namespace OopProject.Controllers
{
    public class AuthController : Controller
    {

        // Registration View
        public IActionResult UserSignup()
        {
            return View();
        }

        // Login View
        public IActionResult UserLogin()
        {
            return View();
        }

        // Logout Action
        public IActionResult Logout()
        {
            // Clear session on logout
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
