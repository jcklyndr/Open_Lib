using Microsoft.AspNetCore.Mvc;
using OopProject.Models;  //iimport pag gagamit na ng models


namespace OopProject.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult PerCategory()
        {
            return View();//temporary for redirect
        }
        public IActionResult BookDetails() => View();

        public IActionResult Success() => View();

        public IActionResult RequestBooks() => View();

    }
}
