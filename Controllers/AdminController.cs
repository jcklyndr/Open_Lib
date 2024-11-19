using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
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

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string AdminName, string Password)
        {
            try
            {
                // Debugging log
                Console.WriteLine($"Received login request for AdminName: {AdminName}");

                // Check if admin exists in the database
                var admin = (await _adminRepository.GetAllAsync()).FirstOrDefault(u => u.AdminName == AdminName);
                if (admin == null)
                {
                    TempData["DebugMessage"] = "Invalid admin name. Admin not found.";
                    Console.WriteLine("Admin not found.");
                    return RedirectToAction("AdminLogin");
                }

                // Verify password using BCrypt
                if (!BCrypt.Net.BCrypt.Verify(Password, admin.Password))
                {
                    TempData["DebugMessage"] = "Invalid password.";
                    Console.WriteLine("Invalid password.");
                    return RedirectToAction("AdminLogin");
                }

                // Create claims for authentication
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.AdminName),
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                // Sign in the admin
                await HttpContext.SignInAsync("AdminScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                // Debugging log
                Console.WriteLine("Admin successfully logged in.");

                // Redirect to Admin Dashboard
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                TempData["DebugMessage"] = "An error occurred during login. Please try again.";
                Console.WriteLine($"Error during login: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("AdminLogin");
            }
        }


        public IActionResult AdminLogout()
        {
            // redirect to Logout in Auth and perform the process of logout for Admin role
            return RedirectToAction("Logout", "Auth", new { role = "Admin" });
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
