using Microsoft.EntityFrameworkCore;
using OopProject.Data;  // DbContext namespace, where OpenLibDbContext is located
using OopProject.Models;

    namespace OopProject.Services
    {
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly OpenLibDbContext _context;

        public Repository(OpenLibDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<T> GetByIdWithCategoriesAsync(int Id)
        {
            if (typeof(T) == typeof(Book))
            {
                // Eager load BookCategories and the related Category
                var book = await _context.Set<Book>()
                    .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                    .FirstOrDefaultAsync(b => b.Id == Id);

                return (T)(object)book; // Casting back to T (Book)
            }
            throw new InvalidOperationException("GetByIdWithCategoriesAsync is only supported for the Book type.");
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            if (typeof(T) == typeof(Category))
            {
                await DeleteCategoryAsync(Id);  // Call the category-specific delete logic
                return;
            }
            else if (typeof(T) == typeof(Book))
            {
                await DeleteBookAsync(Id);  // Call the book-specific delete logic
                return;
            }

            var entity = await GetByIdAsync(Id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<T>> GetAllWithCategoriesAsync()
        {
            if (typeof(T) == typeof(Book))
            {
                var result = await _context.Set<Book>()
                    .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                    .ToListAsync();
                return result.Cast<T>().ToList();
            }
            throw new InvalidOperationException("GetAllWithCategoriesAsync is only supported for the Book type.");
        }

        public async Task DeleteByCompositeKeyAsync(int bookId, int categoryId)
        {
            var bookCategory = await _context.BookCategories
                .FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.CategoryId == categoryId);

            if (bookCategory != null)
            {
                _context.BookCategories.Remove(bookCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BookCategory>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookCategories
                .Where(bc => bc.BookId == bookId)
                .ToListAsync();
        }

        // Implement the DeleteCategoryAsync method
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories
                .Include(c => c.BookCategories) // Include BookCategories to get the associated books
                .ThenInclude(bc => bc.Book)     // Include books from BookCategories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category != null)
            {
                // Remove the relationship between books and this category
                foreach (var bookCategory in category.BookCategories.ToList()) // ToList() to avoid modifying the collection during iteration
                {
                    var book = bookCategory.Book;

                    // Unlink the book from the deleted category
                    _context.BookCategories.Remove(bookCategory);

                    // If the book has no other categories left, delete the book and its requests
                    if (!book.BookCategories.Any()) // Book has no other categories
                    {
                        // Delete the book
                        _context.Books.Remove(book);

                        // Delete the requests for this book
                        var requests = await _context.Requests.Where(r => r.BookId == book.Id).ToListAsync();
                        foreach (var request in requests)
                        {
                            _context.Requests.Remove(request); // Delete the related requests
                        }
                    }
                    else
                    {
                        // Book still has other categories, just unlink it from the deleted category
                        // No need to modify requests here since they are already indirectly linked via book
                    }
                }

                // Now delete the category itself
                _context.Categories.Remove(category);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }


        // Implement the DeleteBookAsync method
        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                // Delete related BookCategory records first
                var bookCategories = await _context.BookCategories
                    .Where(bc => bc.BookId == bookId)
                    .ToListAsync();
                _context.BookCategories.RemoveRange(bookCategories);

                // Delete related Requests for this book
                var requests = await _context.Requests
                    .Where(r => r.BookId == bookId)
                    .ToListAsync();
                _context.Requests.RemoveRange(requests);

                // Then remove the Book itself
                _context.Books.Remove(book);

                await _context.SaveChangesAsync();
            }
        }




    }

}






