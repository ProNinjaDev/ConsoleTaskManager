using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogTaskStatusChange(string userName, int taskId, string taskName, ProjectTaskStatus oldStatus, ProjectTaskStatus newStatus);
        Task<IEnumerable<string>> GetLogsAsync();
    }
} 