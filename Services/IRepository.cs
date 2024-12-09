using OopProject.Models;

namespace OopProject.Services
        {
            public interface IRepository<T> where T : class
            {
                Task<IEnumerable<T>> GetAllAsync();
                Task<T> GetByIdAsync(int Id);
                Task AddAsync(T entity);    
                Task UpdateAsync(T entity);
                Task DeleteAsync(int Id);
                Task<IEnumerable<T>> GetAllWithCategoriesAsync();
                Task DeleteByCompositeKeyAsync(int bookId, int categoryId);
                Task<T> GetByIdWithCategoriesAsync(int Id);
                Task<List<BookCategory>> GetByBookIdAsync(int bookId);
                Task DeleteCategoryAsync(int categoryId);  // Only for Category
                Task DeleteBookAsync(int bookId);



    }
}

