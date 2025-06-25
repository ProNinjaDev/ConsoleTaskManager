using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> SignIn(string login, string password);
    }
}