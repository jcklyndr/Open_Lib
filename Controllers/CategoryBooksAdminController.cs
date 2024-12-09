using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using System.Threading.Tasks;

namespace OopProject.Controllers
{
    public class CategoryBooksAdminController : AdminHeaderController
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRequestRepository _requestRepository;  // Change this to IRequestRepository

        public CategoryBooksAdminController(IRepository<Category> categoryRepository, IRequestRepository requestRepository)
        {
            _categoryRepository = categoryRepository;
            _requestRepository = requestRepository;  // Use IRequestRepository
        }

        public async Task<IActionResult> AllCategoryBooks()
        {
            // Fetch all categories from the repository
            var categories = await _categoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                // Log or debug if data is null or empty
                Console.WriteLine("No admin data retrieved.");
            }
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(category); // Return to the form if there are validation errors
            }

            // Handle the image upload if an image is provided
            if (image != null)
            {
                var imagePath = Path.Combine("wwwroot", "images", "categories", image.FileName);
                if (!Directory.Exists(Path.Combine("wwwroot", "images", "categories")))
                {
                    Directory.CreateDirectory(Path.Combine("wwwroot", "images", "categories"));
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Save the image path in the Category model
                category.Image = "/images/categories/" + image.FileName;
            }

            // Add the category to the database
            await _categoryRepository.AddAsync(category);

            // Set a success message
            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction(nameof(AllCategoryBooks));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategoryBooks(int Id)
        {
            Console.WriteLine("UpdateCategoryBooks method called for Id: " + Id);  // For debugging
            var category = await _categoryRepository.GetByIdAsync(Id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryBooks(Category updatedCategory, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCategory); // Return to the form if there are validation errors
            }

            var category = await _categoryRepository.GetByIdAsync(updatedCategory.Id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return RedirectToAction("AllCategoryBooks");
            }

            try
            {
                // Update category properties
                category.CategoryName = updatedCategory.CategoryName;
                category.Description = updatedCategory.Description;

                // Update image if a new one is provided
                if (image != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(category.Image))
                    {
                        var oldImagePath = Path.Combine("wwwroot", category.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    var newImagePath = Path.Combine("wwwroot", "images", "categories", image.FileName);
                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    category.Image = "/images/categories/" + image.FileName;
                }

                // Update the category in the database
                await _categoryRepository.UpdateAsync(category);

                TempData["SuccessMessage"] = "Category updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating category: {ex.Message}";
            }

            return RedirectToAction("AllCategoryBooks");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return RedirectToAction("AllCategoryBooks");
            }

            try
            {
                // Delete the associated image if it exists
                if (!string.IsNullOrEmpty(category.Image))
                {
                    var imagePath = Path.Combine("wwwroot", category.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Use the service to delete the category and unlink from requests
                await _requestRepository.DeleteCategoryAndUnlinkFromRequestsAsync(id);

                TempData["SuccessMessage"] = "Category and its associated relationships removed successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
            }

            return RedirectToAction("AllCategoryBooks");
        }
    }
    }
