using OopProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using OopProject.Models;

namespace OopProject.Services
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<IEnumerable<Request>> GetAllRequestsWithDetailsAsync();
    }
}
