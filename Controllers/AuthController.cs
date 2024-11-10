using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult UserLogin()
        {
            return View();
        }

        public IActionResult UserSignup()
        {
            return View();
        }
    }
}
