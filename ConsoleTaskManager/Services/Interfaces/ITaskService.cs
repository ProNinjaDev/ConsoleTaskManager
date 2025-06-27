using ConsoleTaskManager.Models;
using ConsoleTaskManager.DTOs;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectTask> CreateTaskAsync(CreateTaskDto taskDto, int employeeId);
        Task<ProjectTask> ChangeTaskStatusAsync(int taskId, ProjectTaskStatus newStatus);
        Task<IEnumerable<ProjectTask>> GetAllTasksAsync();
        Task<IEnumerable<ProjectTask>> GetTasksForEmployeeAsync(int employeeId);
    }
}