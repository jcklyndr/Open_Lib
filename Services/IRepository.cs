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

    }
    }

