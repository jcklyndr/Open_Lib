using Microsoft.AspNetCore.Mvc;
using OopProject.Models; // Ensure your models are correctly referenced // parang import
//using OopProject.Services; 
using System.Diagnostics;

namespace OopProject.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View(); // Pass the categories to the view
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
