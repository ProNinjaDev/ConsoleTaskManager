using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllEmployees(); // TODO: добавить асинхронность
        User? GetUserById(int userId);
        User? GetUserByLogin(string login);
    }
}