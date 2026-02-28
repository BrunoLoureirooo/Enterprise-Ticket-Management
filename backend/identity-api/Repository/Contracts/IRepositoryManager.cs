using System.Threading.Tasks;

namespace backend.Repository.Contracts
{
    public interface IRepositoryManager
    {
        Task SaveAsync();
    }
}
