using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.UI.Handlers.Interfaces;

namespace ConsoleTaskManager.UI.Handlers
{
    public class ManagerActionHandler : IActionHandler
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly ConsoleView _consoleView;
        private readonly ILoggerService _loggerService;

        public ManagerActionHandler(IUserService userService, ITaskService taskService, ConsoleView consoleView, ILoggerService loggerService)
        {
            _userService = userService;
            _taskService = taskService;
            _consoleView = consoleView;
            _loggerService = loggerService;
        }

        public async Task HandleActionAsync(char choice, User currentUser)
        {
            switch (choice)
            {
                case '1':
                    try 
                    {
                        var employees = await _userService.GetAllEmployeesAsync();
                        if (!employees.Any())
                        {
                            _consoleView.DisplayMessage("[ERROR] There are no employees to assign a task to", true);
                            break;
                        }

                        var employeeId = _consoleView.SelectEmployee(employees);
                        if (employeeId is null)
                        {
                            _consoleView.DisplayMessage("[INFO] Task creation cancelled");
                            break;
                        }

                        var taskDto = _consoleView.GetNewTaskDetails();
                        if (string.IsNullOrWhiteSpace(taskDto.Name))
                        {
                            _consoleView.DisplayMessage("[ERROR] Task name cannot be empty", true);
                            break;
                        }

                        var newTask = await _taskService.CreateTaskAsync(taskDto, employeeId.Value);
                        _consoleView.DisplayMessage($"[OK] Task '{newTask.Name}' created and assigned to employee ID {newTask.AssignedEmployeeId}");

                    }
                    catch (UserNotFoundException ex)
                    {
                        _consoleView.DisplayMessage($"[ERROR] {ex.Message}", true);
                    }
                    catch (Exception ex)
                    {
                        _consoleView.DisplayMessage($"[ERROR] An unexpected error occurred: {ex.Message}", true);
                    }
                    break;
                case '2':
                    try
                    {
                        var newUserDetails = _consoleView.GetNewUserDetails();

                        if (string.IsNullOrWhiteSpace(newUserDetails.Login) || string.IsNullOrWhiteSpace(newUserDetails.Password))
                        {
                            _consoleView.DisplayMessage("[ERROR] Login and password cannot be empty", true);
                            break;
                        }

                        var newUser = await _userService.CreateUserAsync(newUserDetails.Login, newUserDetails.Password);
                        _consoleView.DisplayMessage($"[OK] Employee '{newUser.Login}' has been registered");
                    }
                    catch (DuplicateLoginException ex)
                    {
                        _consoleView.DisplayMessage($"[ERROR] {ex.Message}", true);
                    }
                    catch (Exception ex)
                    {
                        _consoleView.DisplayMessage($"Error: {ex.Message}", true);
                    }
                    break;
                case '3':
                    var allTasks = await _taskService.GetAllTasksAsync();
                    _consoleView.DisplayTasks(allTasks, "All tasks");
                    break;
                case '4':
                    var users = await _userService.GetAllUsersAsync();
                    _consoleView.DisplayUsers(users);
                    break;
                case '5':
                    var logs = await _loggerService.GetLogsAsync();
                    _consoleView.DisplayLogs(logs);
                    break;
            }
            Console.WriteLine("\nPress any key to return to the menu");
            Console.ReadKey();
        }
    }
} 