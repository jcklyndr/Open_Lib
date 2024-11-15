using Microsoft.EntityFrameworkCore;
using OopProject.Data;  // DbContext namespace, where OpenLibDbContext is located

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
            var entity = await GetByIdAsync(Id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

