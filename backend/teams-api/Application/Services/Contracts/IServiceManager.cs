namespace teams.Application.Services.Contracts
{
    public interface IServiceManager
    {
        ITeamService TeamService { get; }
        IProjectService ProjectService { get; }
    }
}
