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

Console.WriteLine("TEST START");

var admin = await authService.SignIn("Admin", "Adminpassword555");

if(admin != null) 
{
    Console.WriteLine($"[OK] Sign-in successful! User: {admin.Login}, Role: {admin.Role}");

    var newEmployee = await userService.CreateUserAsync("MyLoveEmployee", "passpass111", UserRole.Employee);
    Console.WriteLine($"[OK] Created employee: {newEmployee.Login}, ID: {newEmployee.Id}");

    var taskDto = new ConsoleTaskManager.DTOs.CreateTaskDto 
    {
        Name = "UI layer",
        Description = "Create a UI layer for this application",
        ProjectId = "Summer"
    };

    var newTask = await taskService.CreateTaskAsync(taskDto, newEmployee.Id);
    Console.WriteLine($"[OK] Created task: '{newTask.Name}' for employee ID {newTask.AssignedEmployeeId}");

    var employeeTasks = await taskService.GetTasksForEmployeeAsync(newEmployee.Id);
    Console.WriteLine($"[INFO] Found {employeeTasks.Count()} tasks for employee {newEmployee.Login}");
}
else 
{
    Console.WriteLine("[ERROR] Failed to sign in as the admin user");
}



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