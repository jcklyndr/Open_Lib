using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace OopProject.Controllers
{
    public class AdminController : AdminHeaderController
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Request> _requestRepository;


        public AdminController(IRepository<Admin> adminRepository, IRepository<Category> categoryRepository,
            IRepository<Book> bookRepository, IRepository<Request> requestRepository)
        {
            _adminRepository = adminRepository;
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _requestRepository = requestRepository;
        }
        public async Task<IActionResult> Index()
        {
            // Retrieve the count ng admin,category,books and request
            var adminCount = (await _adminRepository.GetAllAsync()).Count();
            var categoryCount = (await _categoryRepository.GetAllAsync()).Count();
            var bookCount = (await _bookRepository.GetAllAsync()).Count();
            var requestCount = (await _requestRepository.GetAllAsync()).Count();



            // Pass the counts to the view
            ViewData["AdminCount"] = adminCount;
            ViewData["CategoryCount"] = categoryCount;
            ViewData["BookCount"] = bookCount;
            ViewData["RequestCount"] = requestCount;

            return View();
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
            // Retrieve admin from db
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

        public async Task<IActionResult> AllAdmin()
        {
            var admins = await _adminRepository.GetAllAsync();

            if (admins == null || !admins.Any())
            {
                // Log or debug if data is null or empty
                Console.WriteLine("No admin data retrieved.");
            }
            return View(admins);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAdmin(int Id)
        {
            var existingAdmin = await _adminRepository.GetByIdAsync(Id); // Fetch admin by ID
            if (existingAdmin == null)
            {
                return NotFound();
            }

            return View(existingAdmin); // Pass the admin to the view
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdmin(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }
            var existingAdmin = await _adminRepository.GetByIdAsync(admin.Id);
            if (existingAdmin == null)
            {
                return NotFound();
            }
            // Update the fields
            existingAdmin.AdminName = admin.AdminName;
            existingAdmin.Email = admin.Email;


            if (!string.IsNullOrEmpty(admin.Password))
            {
                existingAdmin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password); ;
            }

            await _adminRepository.UpdateAsync(existingAdmin); // Update admin 
            TempData["Success"] = "Admin updated successfully!";
            return RedirectToAction("AllAdmin");
        }

        // Handle the Update Form Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdmins(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin); // Return with validation
            }

            var existingAdmin = await _adminRepository.GetByIdAsync(admin.Id);
            if (existingAdmin == null)
            {
                return NotFound();
            }

            // Update fields
            existingAdmin.AdminName = admin.AdminName;
            existingAdmin.Email = admin.Email;

            if (!string.IsNullOrEmpty(admin.Password))
            {
                existingAdmin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password); // Hash new password
            }

            await _adminRepository.UpdateAsync(existingAdmin); // Update the admin

            TempData["SuccessMessage"] = "Admin updated successfully!";

            return RedirectToAction("AllAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdmin(int Id)
        {
            var admin = await _adminRepository.GetByIdAsync(Id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdminConfirmed(int Id)
        {
            var admin = await _adminRepository.GetByIdAsync(Id);
            if (admin == null)
            {
                // Admin not found
                TempData["ErrorMessage"] = "Admin not found.";
                return RedirectToAction("AllAdmin");
            }
            try
            {
                await _adminRepository.DeleteAsync(Id); //DeleteAsync method in the Repository
                TempData["SuccessMessage"] = "Admin deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting admin: {ex.Message}";
            }
            return RedirectToAction("AllAdmin");
        }
    }
}
