using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleTaskManager.UI.Handlers
{
    public class EmployeeActionHandler
    {
        private readonly ITaskService _taskService;
        private readonly ConsoleView _consoleView;

        public EmployeeActionHandler(ITaskService taskService, ConsoleView consoleView)
        {
            _taskService = taskService;
            _consoleView = consoleView;
        }

        public async Task HandleActionAsync(char choice, int employeeId)
        {
            switch (choice)
            {
                case '1':
                    var myTasks = await _taskService.GetTasksForEmployeeAsync(employeeId);
                    _consoleView.DisplayTasks(myTasks, "My tasks");
                    break;
                case '2':
                    try
                    {
                        var tasks = await _taskService.GetTasksForEmployeeAsync(employeeId);
                        if (!tasks.Any())
                        {
                            _consoleView.DisplayMessage("[INFO] You have no tasks assigned to you");
                            break;
                        }

                        _consoleView.DisplayTasks(tasks, "My tasks");

                        Console.Write("\nEnter the ID of the task to update (or 0 to cancel): ");
                        string strTaskId = Console.ReadLine() ?? string.Empty;
                        if (!int.TryParse(strTaskId, out int taskId) || taskId == 0)
                        {
                            _consoleView.DisplayMessage("Operation cancelled");
                            break;
                        }

                        var newStatus = _consoleView.SelectTaskStatus();
                        if (newStatus is null)
                        {
                            _consoleView.DisplayMessage("Operation cancelled");
                            break;
                        }

                        var updatedTask = await _taskService.ChangeTaskStatusAsync(taskId, newStatus.Value);
                        _consoleView.DisplayMessage($"[OK] Status for task ID {updatedTask.Id} has been updated to {updatedTask.Status}");
                    }
                    catch (TaskNotFoundException ex)
                    {
                        _consoleView.DisplayMessage($"[ERROR] {ex.Message}", true);
                    }
                    catch (Exception ex)
                    {
                        _consoleView.DisplayMessage($"[ERROR] An unexpected error occurred: {ex.Message}", true);
                    }
                    break;
            }
            Console.WriteLine("\nPress any key to return to the menu");
            Console.ReadKey();
        }
    }
} 