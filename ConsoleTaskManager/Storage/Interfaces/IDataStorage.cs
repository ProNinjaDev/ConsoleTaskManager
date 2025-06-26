using ConsoleTaskManager.Models;
using System.Threading.Tasks;

namespace ConsoleTaskManager.Storage.Interfaces
{
    public interface IDataStorage
    {
        Task<IEnumerable<User>> LoadUsersAsync();
        Task<IEnumerable<ProjectTask>> LoadTasksAsync();
        Task SaveChangesAsync(IEnumerable<User> users, IEnumerable<ProjectTask> tasks);
    }
}