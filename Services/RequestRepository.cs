// In RequestRepository.cs
using OopProject.Models;
using Microsoft.EntityFrameworkCore;
using OopProject.Data;

namespace OopProject.Services
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly OpenLibDbContext _context;

        public RequestRepository(OpenLibDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom method for fetching requests with Book and Category details
        public async Task<IEnumerable<Request>> GetAllRequestsWithDetailsAsync()
        {
            return await _context.Requests
                .Include(r => r.Book)
                .ThenInclude(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .ToListAsync();
        }

        // New method to handle category deletion and unlinking it from requests
        public async Task DeleteCategoryAndUnlinkFromRequestsAsync(int categoryId)
        {
            // Load the category with its associated BookCategories and the related Book entities
            var category = await _context.Categories
                .Include(c => c.BookCategories)  // Load related BookCategories
                .ThenInclude(bc => bc.Book)     // Ensure the Book is loaded
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            // Fetch the BookCategories associated with the category
            var bookCategories = await _context.BookCategories
                .Where(bc => bc.CategoryId == categoryId)
                .Include(bc => bc.Book)  // Include the Book entity for each BookCategory
                .ToListAsync();

            foreach (var bookCategory in bookCategories)
            {
                var book = bookCategory.Book;

                // Check if the book is null before accessing its properties
                if (book == null)
                {
                    continue; // If the book is null, skip this BookCategory
                }

                // Remove the BookCategory relationship (unlink the category from the book)
                _context.BookCategories.Remove(bookCategory);

                // Check if the book has any remaining categories
                if (!book.BookCategories.Any()) // No categories left for the book
                {
                    // If the book has no other categories, delete the requests associated with it
                    var requestsToRemove = await _context.Requests
                        .Where(r => r.BookId == book.Id)
                        .ToListAsync();

                    foreach (var request in requestsToRemove)
                    {
                        _context.Requests.Remove(request); // Remove requests associated with the book
                    }

                    // Now delete the book since it has no categories left
                    _context.Books.Remove(book);
                }
            }

            // Now delete the category itself
            _context.Categories.Remove(category);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
    }
