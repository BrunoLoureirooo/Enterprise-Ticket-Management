namespace backend.Application.Services.Contracts
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        IUserService UserService { get; }
        IRoleService RoleService { get; }
    }
}