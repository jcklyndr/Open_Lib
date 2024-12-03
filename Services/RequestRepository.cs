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
    }
}
