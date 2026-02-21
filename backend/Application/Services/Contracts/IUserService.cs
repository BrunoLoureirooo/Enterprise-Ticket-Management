using backend.Entities.Models;

namespace backend.Application.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>?> GetUsers();
    }
}
