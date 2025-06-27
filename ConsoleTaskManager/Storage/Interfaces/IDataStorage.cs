using ConsoleTaskManager.Models;
using System.Threading.Tasks;

namespace ConsoleTaskManager.Storage.Interfaces
{
    public interface IDataStorage
    {
        Task<IEnumerable<User>> LoadUsersAsync();
        Task<IEnumerable<ProjectTask>> LoadTasksAsync();
        Task SaveUsersAsync(IEnumerable<User> users);
        Task SaveTasksAsync(IEnumerable<ProjectTask> tasks);
    }
}