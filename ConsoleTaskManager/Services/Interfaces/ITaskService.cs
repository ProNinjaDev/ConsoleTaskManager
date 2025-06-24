using ConsoleTaskManager.Models;
using ConsoleTaskManager.DTOs;

namespace ConsoleTaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        ProjectTask CreateTask(CreateTaskDto taskDto, int employeeId); // TODO: добавить асинхронность

        IEnumerable<ProjectTask> GetTasksForEmployee(int employeeId);

        ProjectTask? ChangeTaskStatus(int taskId, ProjectTaskStatus newStatus);

        ProjectTask? AssignTask(int taskId, int employeeId); 
    }
}