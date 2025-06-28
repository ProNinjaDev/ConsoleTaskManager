using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services.Interfaces;


namespace ConsoleTaskManager.Services
{
    public class FileLoggerService : ILoggerService
    {
        private readonly string _logFilePath;

        public FileLoggerService(string logFilePath = "task_log.txt")
        {
            _logFilePath = logFilePath;
        }

        public void LogTaskStatusChange(string userName, int taskId, string taskName, ProjectTaskStatus oldStatus, ProjectTaskStatus newStatus)
        {
            var logTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            var logEntry = $"[{logTime}] - User '{userName}' changed status for task '{taskName}' (ID: {taskId}) from '{oldStatus}' to '{newStatus}'{Environment.NewLine}";

            File.AppendAllText(_logFilePath, logEntry);
        }

        public async Task<IEnumerable<string>> GetLogsAsync()
        {
            if (!File.Exists(_logFilePath))
            {
                return Enumerable.Empty<string>();
            }

            return await File.ReadAllLinesAsync(_logFilePath);
        }
    }
} 