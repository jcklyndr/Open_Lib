using OopProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using OopProject.Models;

namespace OopProject.Services
{
    public interface IRequestRepository : IRepository<Request>
    {
        // Other methods for managing requests
        Task<IEnumerable<Request>> GetAllRequestsWithDetailsAsync();

        // Add the custom method for deleting categories and unlinking from requests
        Task DeleteCategoryAndUnlinkFromRequestsAsync(int categoryId);
    }
}

