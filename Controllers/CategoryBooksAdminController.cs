using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using System.Threading.Tasks;

namespace OopProject.Controllers
{
    public class CategoryBooksAdminController : AdminHeaderController
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryBooksAdminController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> AllCategoryBooks()
        {
            // Fetch categories
            var categories = await _categoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                // Log pag empty
                Console.WriteLine("No data retrieved.");
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
                return View(category);
            }
            // Handle image
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

                // Save image path
                category.Image = "/images/categories/" + image.FileName;
            }
            await _categoryRepository.AddAsync(category);

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
                return View(updatedCategory);
            }

            var category = await _categoryRepository.GetByIdAsync(updatedCategory.Id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return RedirectToAction("AllCategoryBooks");
            }
            try
            {
                // Update category
                category.CategoryName = updatedCategory.CategoryName;
                category.Description = updatedCategory.Description;

                // Update image if provided
                if (image != null)
                {
                    // Delete old image if meron
                    if (!string.IsNullOrEmpty(category.Image))
                    {
                        var oldImagePath = Path.Combine("wwwroot", category.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // Save new image
                    var newImagePath = Path.Combine("wwwroot", "images", "categories", image.FileName);
                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    category.Image = "/images/categories/" + image.FileName;
                }
                // Update the category
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
        public async Task<IActionResult> DeleteCategoryConfirmed(int Id)
        {
            var category = await _categoryRepository.GetByIdAsync(Id);
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
                    var imagePath = Path.Combine("wwwroot", category.Image.TrimStart('/')); // Convert relative path to physical path
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                // Deleterecord
                await _categoryRepository.DeleteAsync(Id);

                TempData["SuccessMessage"] = "Category and its image deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
            }
            return RedirectToAction("AllCategoryBooks");
        }
    }
}
