using backend.Entities.Models;
using backend.Entities.DataTransferObjects;
using backend.Entities.DataTransferObjects.Users;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<ApplicationUser?> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateToken(ApplicationUser user, bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
