using OopProject.Models;

namespace OopProject.Services
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<IEnumerable<Request>> GetAllRequestsWithDetailsAsync();
    }
}
