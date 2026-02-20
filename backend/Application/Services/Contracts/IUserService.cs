using backend.Entities.Models;
using backend.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace backend.Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>?> GetUsers();
    }
}
