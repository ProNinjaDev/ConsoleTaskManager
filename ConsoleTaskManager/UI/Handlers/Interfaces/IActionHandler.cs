using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.UI.Handlers.Interfaces
{
    public interface IActionHandler
    {
        Task HandleActionAsync(char choice, User currentUser);
    }
} 