using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface IAuthService
    {
        User SignUp(string login, string password, UserRole role = UserRole.Employee);
        User? SingIn(string login, string password);
    }
}