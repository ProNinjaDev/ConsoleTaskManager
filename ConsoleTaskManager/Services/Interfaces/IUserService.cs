using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllEmployeesAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByLoginAsync(string login);
        Task<User> CreateUserAsync(string login, string password, UserRole role = UserRole.Employee);
    }
}