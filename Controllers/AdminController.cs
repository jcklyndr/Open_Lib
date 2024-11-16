using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace OopProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<Admin> _adminRepository;

        public AdminController(IRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
        }
        //for redirect sa page 
        public IActionResult CreateAdmin()
        {
            return View();
        }


        // POST: /Auth/AdminSignup or  CreateAdmin
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _adminRepository.GetAllAsync();
                if (existingUser.Any(u => u.Email == admin.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(admin);
                }

                // Hash password and save the admin
                admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);
                await _adminRepository.AddAsync(admin);

                // Success message
                TempData["SuccessMessage"] = "Registration successful for this admin.";
                return RedirectToAction(nameof(CreateAdmin)); // Redirect to the same page to clear the form
            }

            // If ModelState is invalid, redisplay the form with validation messages
            return View(admin);
        }


        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllAdmin()
        {
            return View();
        }

        public IActionResult UpdateAdmin()
        {
            return View();
        }


        public IActionResult AllCategoryBooks()
        {
            return View();
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

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

        public IActionResult AllRequest()
        {
            return View();
        }
    }
}
