using ConsoleTaskManager.DTOs;
using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage.Interfaces;

namespace ConsoleTaskManager.Services
{
    public class TaskService : ITaskService 
    {
        private readonly IDataStorage _dataStorage;

        public TaskService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<ProjectTask> CreateTaskAsync(CreateTaskDto taskDto, int employeeId) 
        {
            var tasks = (await _dataStorage.LoadTasksAsync()).ToList();

            var users = await _dataStorage.LoadUsersAsync();
            if (!users.Any(u => u.Id == employeeId && u.Role == UserRole.Employee))
            {
                throw new UserNotFoundException(employeeId);
            }

            int newId;
            if (tasks.Count > 0) 
            {
                int maxId = tasks.Max(t => t.Id);
                newId = maxId + 1;
            }
            else 
            {
                newId = 1;
            }

            var newTask = new ProjectTask
            {
                Id = newId,
                Name = taskDto.Name,
                Description = taskDto.Description,
                ProjectId = taskDto.ProjectId,
                Status = ProjectTaskStatus.ToDo,
                AssignedEmployeeId = employeeId
            };

            tasks.Add(newTask);
            await _dataStorage.SaveTasksAsync(tasks);

            return newTask;
        }

        public async Task<ProjectTask> ChangeTaskStatusAsync(int taskId, ProjectTaskStatus newStatus) 
        {
            var tasks = (await _dataStorage.LoadTasksAsync()).ToList();
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskToUpdate == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            taskToUpdate.Status = newStatus;

            await _dataStorage.SaveTasksAsync(tasks);

            return taskToUpdate;
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksForEmployeeAsync(int employeeId)
        {
            var tasks = await _dataStorage.LoadTasksAsync();
            return tasks.Where(t => t.AssignedEmployeeId == employeeId);
        }

        public async Task<IEnumerable<ProjectTask>> GetAllTasksAsync(ProjectTaskStatus? statusFilter = null)
        {
            var tasks = await _dataStorage.LoadTasksAsync();
            if (statusFilter is not null)
            {
                return tasks.Where(t => t.Status == statusFilter.Value);
            }
            return tasks;
        }
    }
}