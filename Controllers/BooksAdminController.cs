using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    public class BooksAdminController : AdminHeaderController
    {
        public IActionResult AllBooks()
        {
            return View();
        }

        public IActionResult CreateBooks()
        {
            return View();
        }

        public IActionResult AddBooks()
        {
            return View();
        }
    }
}
