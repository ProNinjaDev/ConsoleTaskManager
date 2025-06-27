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
var actionHandlerFactory = new ActionHandlerFactory(userService, taskService, consoleView);


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
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
            continue;
        }

        consoleView.DisplayMessage($"Sign-in successful! Welcome {currentUser.Login} ({currentUser.Role})");
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();

        await UserProcessingLoop(currentUser);
        
    }
}

async Task UserProcessingLoop(User currentUser)
{
    while (true) 
    {
        char choice;

        if (currentUser.Role == UserRole.Manager)
        {
            choice = consoleView.DisplayManagerMenu();
        }
        else 
        {
            choice = consoleView.DisplayEmployeeMenu();
        }

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