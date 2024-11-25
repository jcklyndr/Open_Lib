using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace OopProject.Controllers
{
    public class AdminController : AdminHeaderController
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
        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(string AdminName, string Password, string? ReturnUrl = null)
        {
            // Retrieve admin from the database
            var admin = (await _adminRepository.GetAllAsync())
                .FirstOrDefault(a => a.AdminName == AdminName);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(Password, admin.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            // Create admin claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, admin.AdminName),
        new Claim(ClaimTypes.Role, "Admin")
    };
            var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");

            // Debugging: Log authentication success
            Console.WriteLine($"Authenticated as: {admin.AdminName}, Role: Admin");

            // Sign in with AdminScheme
            await HttpContext.SignInAsync("AdminScheme", new ClaimsPrincipal(claimsIdentity));

            // Redirect to the original page or admin dashboard
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public async Task<IActionResult> AdminLogout()
        {

            // Sign out from AdminScheme
            await HttpContext.SignOutAsync("AdminScheme");
            TempData["SuccessMessage"] = "Admin logged out successfully.";
            // Redirect to the AdminLogin page after logout
            return RedirectToAction("AdminLogin", "Admin");
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

/*        public IActionResult AllRequest()
        {
            return View();
        }*/
    }
}
