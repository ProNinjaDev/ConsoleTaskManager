using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage;
using ConsoleTaskManager.Storage.Interfaces;
using ConsoleTaskManager.UI;
using ConsoleTaskManager.UI.Handlers;

IDataStorage dataStorage = new JsonDataStorage("users.json", "tasks.json");

IAuthService authService = new AuthService(dataStorage);
IUserService userService = new UserService(dataStorage);
ITaskService taskService = new TaskService(dataStorage);
ConsoleView consoleView = new ConsoleView();
ILoggerService loggerService = new FileLoggerService();
var actionHandlerFactory = new ActionHandlerFactory(userService, taskService, consoleView, loggerService);


await InitializeApplication();
await RunApplication();

async Task RunApplication()
{
    while (true)
    {
        var credentials = consoleView.GetLoginCredentials();
        var currentUser = await authService.SignIn(credentials.Login, credentials.Password);

        if (currentUser is null)
        {
            consoleView.DisplayMessage("[ERROR] Invalid login or password", true);
            consoleView.WaitForAnyKey();
            continue;
        }

        consoleView.DisplayMessage($"Sign-in successful! Welcome {currentUser.Login} ({currentUser.Role})");
        consoleView.WaitForAnyKey();

        await UserProcessingLoop(currentUser);
        
    }
}

async Task UserProcessingLoop(User currentUser)
{
    while (true) 
    {
        string menuTitle;
        Dictionary<char, string> menuOptions;

        if (currentUser.Role == UserRole.Manager)
        {
            menuTitle = "Manager's Menu";
            menuOptions = new Dictionary<char, string>
            {
                { '1', "Create a new task" },
                { '2', "Register a new employee" },
                { '3', "View all tasks" },
                { '4', "View all users" },
                { '5', "View task activity log" },
                { '0', "Log out" }
            };
        }
        else 
        {
            menuTitle = "Employee's Menu";
            menuOptions = new Dictionary<char, string>
            {
                { '1', "View my assigned tasks" },
                { '2', "Change a task's status" },
                { '0', "Log out" }
            };
        }

        char choice = consoleView.DisplayMenu(menuTitle, menuOptions);

        if (choice == '0')
        {
            consoleView.DisplayMessage("You have been logged out");
            break;
        }

        var handler = actionHandlerFactory.CreateHandler(currentUser.Role);
        await handler.HandleActionAsync(choice, currentUser);
    }
}

async Task InitializeApplication() 
{
    var existingUsers = await dataStorage.LoadUsersAsync();

    if (!existingUsers.Any()) 
    {
        consoleView.DisplayMessage("User base is empty");
        await userService.CreateUserAsync("Admin", "Adminpassword555", UserRole.Manager);
        consoleView.DisplayMessage("Managing user is created by default");
    }
    else 
    {
        consoleView.DisplayMessage("User base has been discovered");
    }
}