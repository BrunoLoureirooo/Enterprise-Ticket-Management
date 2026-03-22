namespace ticket.Application.Services.Contracts
{
    public interface IServiceManager
    {
        ITicketService TicketService { get; }
    }
}
