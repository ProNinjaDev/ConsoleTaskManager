using ConsoleTaskManager.Models;
using ConsoleTaskManager.DTOs;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectTask> CreateTaskAsync(CreateTaskDto taskDto, int employeeId); // TODO: добавить асинхронность

        Task<IEnumerable<ProjectTask>> GetTasksForEmployeeAsync(int employeeId);

        Task<ProjectTask?> ChangeTaskStatusAsync(int taskId, ProjectTaskStatus newStatus);

        Task<ProjectTask?> AssignTaskAsync(int taskId, int employeeId); 
    }
}