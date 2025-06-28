using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.UI.Handlers.Interfaces;

namespace ConsoleTaskManager.UI.Handlers
{
    public class ActionHandlerFactory
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly ConsoleView _consoleView;
        private readonly ILoggerService _loggerService;

        public ActionHandlerFactory(IUserService userService, ITaskService taskService, ConsoleView consoleView, ILoggerService loggerService)
        {
            _userService = userService;
            _taskService = taskService;
            _consoleView = consoleView;
            _loggerService = loggerService;
        }

        public IActionHandler CreateHandler(UserRole role)
        {
            switch (role)
            {
                case UserRole.Manager:
                    return new ManagerActionHandler(_userService, _taskService, _consoleView, _loggerService);
                case UserRole.Employee:
                    return new EmployeeActionHandler(_taskService, _consoleView, _loggerService);
                default:
                    throw new NotSupportedException($"Role '{role}' is not supported by the factory.");
            }
        }
    }
} 