using AutoMapper;
using backend.Entities;
using backend.Entities.Models;
using backend.Application.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using backend.Repository.Contracts;

namespace backend.Application.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IRoleService> _roleService;

        public ServiceManager(
            IRepositoryManager repositoryManager,
            ILoggerManager logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtConfiguration> configuration,
            IEnumerable<IClaimEnricher> claimEnrichers,
            ITokenRevocationService revocation)
        {
            _authenticationService = new Lazy<IAuthenticationService>(() =>
                new AuthenticationService(logger, mapper, userManager, roleManager, configuration, claimEnrichers, revocation));

            _userService = new Lazy<IUserService>(() =>
                new UserService(logger, mapper, userManager));

            _roleService = new Lazy<IRoleService>(() =>
                new RoleService(roleManager, userManager, revocation, configuration));
        }

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserService UserService => _userService.Value;
        public IRoleService RoleService => _roleService.Value;
    }
}