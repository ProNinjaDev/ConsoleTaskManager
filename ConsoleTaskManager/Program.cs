using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage;
using ConsoleTaskManager.Storage.Interfaces;
using ConsoleTaskManager.UI;

IDataStorage dataStorage = new JsonDataStorage("users.json", "tasks.json");

IAuthService authService = new AuthService(dataStorage);
IUserService userService = new UserService(dataStorage);
ITaskService taskService = new TaskService(dataStorage);
ConsoleView consoleView = new ConsoleView();


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

        if (currentUser.Role == UserRole.Manager)
        {
            await HandleManagerAction(choice);
        }
        else
        {
            await HandleEmployeeAction(choice, currentUser.Id);
        }
    }
}

async Task HandleManagerAction(char choice)
{
    switch (choice)
    {
        case '1':
            try 
            {
                var employees = await userService.GetAllEmployeesAsync();
                if (!employees.Any())
                {
                    consoleView.DisplayMessage("[ERROR] There are no employees to assign a task to", true);
                    break;
                }

                var employeeId = consoleView.SelectEmployee(employees);
                if (employeeId is null)
                {
                    consoleView.DisplayMessage("[INFO] Task creation cancelled");
                    break;
                }

                var taskDto = consoleView.GetNewTaskDetails();
                if (string.IsNullOrWhiteSpace(taskDto.Name))
                {
                    consoleView.DisplayMessage("[ERROR] Task name cannot be empty", true);
                    break;
                }

                var newTask = await taskService.CreateTaskAsync(taskDto, employeeId.Value);
                consoleView.DisplayMessage($"[OK] Task '{newTask.Name}' created and assigned to employee ID {newTask.AssignedEmployeeId}");

            }
            catch (Exception ex)
            {
                consoleView.DisplayMessage($"[ERROR] An unexpected error occurred: {ex.Message}", true);
            }
            break;
        case '2':
            try
            {
                var newUserDetails = consoleView.GetNewUserDetails();

                if (string.IsNullOrWhiteSpace(newUserDetails.Login) || string.IsNullOrWhiteSpace(newUserDetails.Password))
                {
                    consoleView.DisplayMessage("[ERROR] Login and password cannot be empty", true);
                    break;
                }

                var newUser = await userService.CreateUserAsync(newUserDetails.Login, newUserDetails.Password);
                consoleView.DisplayMessage($"[OK] Employee '{newUser.Login}' has been registered");
            }
            catch (Exception ex)
            {
                consoleView.DisplayMessage($"Error: {ex.Message}", true);
            }
            break;
        case '3':
            var allTasks = await taskService.GetAllTasksAsync();
            consoleView.DisplayTasks(allTasks, "All tasks");
            break;
        case '4':
            var users = await userService.GetAllUsersAsync();
            consoleView.DisplayUsers(users);
            break;
    }
    Console.WriteLine("\nPress any key to return to the menu");
    Console.ReadKey();
}

async Task HandleEmployeeAction(char choice, int employeeId)
{
    switch (choice)
    {
        case '1':
            var myTasks = await taskService.GetTasksForEmployeeAsync(employeeId);
            consoleView.DisplayTasks(myTasks, "My tasks");
            break;
        case '2':
            try
            {
                var tasks = await taskService.GetTasksForEmployeeAsync(employeeId);
                if (!tasks.Any())
                {
                    consoleView.DisplayMessage("[INFO] You have no tasks assigned to you");
                    break;
                }

                consoleView.DisplayTasks(tasks, "My tasks");

                Console.Write("\nEnter the ID of the task to update (or 0 to cancel): ");
                string strTaskId = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(strTaskId, out int taskId) || taskId == 0)
                {
                    consoleView.DisplayMessage("Operation cancelled");
                    break;
                }

                if (!tasks.Any(t => t.Id == taskId))
                {
                    consoleView.DisplayMessage("[ERROR] Invalid ID", true);
                    break;
                }

                var newStatus = consoleView.SelectTaskStatus();
                if (newStatus is null)
                {
                    consoleView.DisplayMessage("Operation cancelled");
                    break;
                }

                await taskService.ChangeTaskStatusAsync(taskId, newStatus.Value);
                consoleView.DisplayMessage($"[OK] Status for task ID {taskId} has been updated to {newStatus.Value}");
            }
            catch (Exception ex)
            {
                consoleView.DisplayMessage($"[ERROR] An unexpected error occurred: {ex.Message}", true);
            }
            break;
    }
    Console.WriteLine("\nPress any key to return to the menu");
    Console.ReadKey();
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