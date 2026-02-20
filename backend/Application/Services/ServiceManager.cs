using AutoMapper;
using backend.Entities;
using backend.Entities.Models;
using backend.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using backend.Repository.Contracts;

namespace backend.Application.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IUserService> _userService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, 
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtConfiguration> configuration)
        {
            _authenticationService = new Lazy<IAuthenticationService>(() =>new AuthenticationService(logger, mapper, userManager, roleManager, configuration));
            _userService = new Lazy<IUserService>(() =>new UserService(logger, mapper, userManager));
        }
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserService UserService => _userService.Value;
    }
}