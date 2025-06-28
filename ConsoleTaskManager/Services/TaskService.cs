using ConsoleTaskManager.DTOs;
using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage.Interfaces;
using ConsoleTaskManager.Utils;

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

            int newId = IdGenerator.GenerateNextId(tasks, t => t.Id);

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

        public async Task<IEnumerable<ProjectTask>> GetAllTasksAsync(ProjectTaskStatus? statusFilter = null, TaskSortField? sortBy = null, SortDirection? sortDirection = null)
        {
            var tasks = await _dataStorage.LoadTasksAsync();
            
            if (statusFilter is not null)
            {
                tasks = tasks.Where(t => t.Status == statusFilter.Value);
            }

            if (sortBy is not null)
            {
                if (sortDirection == SortDirection.Descending)
                {
                    switch (sortBy)
                    {
                        case TaskSortField.Name:
                            tasks = tasks.OrderByDescending(t => t.Name);
                            break;
                        case TaskSortField.Status:
                            tasks = tasks.OrderByDescending(t => t.Status);
                            break;
                        default:
                            tasks = tasks.OrderByDescending(t => t.Id);
                            break;
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case TaskSortField.Name:
                            tasks = tasks.OrderBy(t => t.Name);
                            break;
                        case TaskSortField.Status:
                            tasks = tasks.OrderBy(t => t.Status);
                            break;
                        default:
                            tasks = tasks.OrderBy(t => t.Id);
                            break;
                    }
                }
            }

            return tasks;
        }
    }
}