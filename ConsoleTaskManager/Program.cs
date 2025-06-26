using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage;
using ConsoleTaskManager.Storage.Interfaces;

IDataStorage dataStorage = new JsonDataStorage("users.json", "tasks.json");

IAuthService authService = new AuthService(dataStorage);
IUserService userService = new UserService(dataStorage);
ITaskService taskService = new TaskService(dataStorage);


await InitializeApplication();

// TODO: добавить меню

async Task InitializeApplication() 
{
    var existingUsers = await dataStorage.LoadUsersAsync();

    if (!existingUsers.Any()) 
    {
        Console.WriteLine("User base is empty");
        await userService.CreateUserAsync("Admin", "Adminpassword555", UserRole.Manager);
        Console.WriteLine("Managing user is created by default");
    }
    else 
    {
        Console.WriteLine("User base has been discovered");
    }
}