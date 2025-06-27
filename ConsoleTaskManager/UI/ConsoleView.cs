using ConsoleTaskManager.Models;
using ConsoleTaskManager.DTOs;

using System.Text;

namespace ConsoleTaskManager.UI 
{
    public class ConsoleView 
    {
        public void DisplayMessage(string message, bool isError = false)
        {
            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public (string Login, string Password) GetLoginCredentials()
        {
            Console.Clear();
            Console.WriteLine("[INFO] Log in to the system");
            
            Console.Write("Enter your login: ");
            string login = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter your password: ");
            string password = ReadPassword();
            Console.WriteLine();

            return (login, password);
        }

        private string ReadPassword()
        {
            var passwordBuilder = new StringBuilder();

            while (true)
            {
                var keyPress = Console.ReadKey(true);

                if(keyPress.Key == ConsoleKey.Enter)
                {
                    break;
                }

                else if(keyPress.Key == ConsoleKey.Backspace && passwordBuilder.Length > 0)
                {
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                    Console.Write("\b \b"); // визуально стереть символ в консоли
                }
                else if (!char.IsControl(keyPress.KeyChar))
                {
                    passwordBuilder.Append(keyPress.KeyChar);
                    Console.Write("*");
                }
            }
            return passwordBuilder.ToString();

        }

        public char DisplayManagerMenu()
        {
            Console.Clear();
            Console.WriteLine("== Manager's Menu ==");
            Console.WriteLine("Select an action:");
            Console.WriteLine("1. Create a new task");
            Console.WriteLine("2. Register a new employee");
            Console.WriteLine("3. View all tasks");
            Console.WriteLine("4. View all users");
            Console.WriteLine("0. Log out of the system");
            Console.Write("Your choice: ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                char choice = keyPress.KeyChar;

                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == '0')
                {
                    Console.WriteLine(choice);
                    return choice;
                }
            }
        }

        public char DisplayEmployeeMenu()
        {
            Console.Clear();
            Console.WriteLine("== Employee's Menu ==");
            Console.WriteLine("Select an action:");
            Console.WriteLine("1. View my assigned tasks");
            Console.WriteLine("2. Change a task's status");
            Console.WriteLine("0. Log out of the system");
            Console.Write("Your choice: ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                char choice = keyPress.KeyChar;

                if (choice == '1' || choice == '2' || choice == '0')
                {
                    Console.WriteLine(choice);
                    return choice;
                }
            }
        }

        public void DisplayUsers(IEnumerable<User> users)
        {
            Console.Clear();
            Console.WriteLine("== All Registered Users ==");

            if (!users.Any())
            {
                Console.WriteLine("No users found in the system");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Login",-20} {"Role",-10}");
            Console.WriteLine(new string('-', 37)); // рисует сепаратор

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id,-5} {user.Login,-20} {user.Role,-10}");
            }
        }

        public (string Login, string Password) GetNewUserDetails() // FIXME: можно объединить с GetLoginCredentials()
        {
            Console.Clear();
            Console.WriteLine("== Register New Employee ==");

            Console.Write("Enter login for the new employee: ");
            string login = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter password for the new employee: ");
            string password = ReadPassword();
            Console.WriteLine();

            return (login, password);
        }

        public void DisplayTasks(IEnumerable<ProjectTask> tasks, string title)
        {
            Console.Clear();
            Console.WriteLine($"== {title} =="); // FIXME: исправить формат вывода

            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Status",-12} {"AssignedTo",-12}");
            Console.WriteLine(new string('-', 56));

            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id,-5} {task.Name,-25} {task.Status,-12} {"Emp. " + task.AssignedEmployeeId,-12}");
            }
        }

        public CreateTaskDto GetNewTaskDetails()
        {
            Console.WriteLine("\n== Enter Task Details ==");
            
            Console.Write("Enter task name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter task description: ");
            string description = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter project ID: ");
            string projectId = Console.ReadLine() ?? string.Empty;

            return new CreateTaskDto
            {
                Name = name,
                Description = description,
                ProjectId = projectId
            };
        }

        public int? SelectEmployee(IEnumerable<User> employees)
        {
            Console.Clear();
            Console.WriteLine("== Assign Task to Employee ==");
            Console.WriteLine("\nAvailable employees:");
            
            Console.WriteLine($"{"ID",-5} {"Login",-20}");
            Console.WriteLine(new string('-', 27));

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id,-5} {employee.Login,-20}");
            }
            Console.WriteLine(new string('-', 27));

            Console.Write("\nEnter the ID of the employee to assign this task to (or 0 to cancel): ");

            while(true)
            {
                string choiceId = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(choiceId, out int employeeId))
                {
                    if (employeeId == 0)
                    {
                        return null;
                    }
                    
                    if (employees.Any(e => e.Id == employeeId))
                    {
                        return employeeId;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Invalid ID. Please select a valid employee ID: ");
                    Console.ResetColor();
                    
                }
            }
        }

        public ProjectTaskStatus? SelectTaskStatus()
        {
            Console.WriteLine("\n-- Select New Status --");
            Console.WriteLine("1. ToDo");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Done");
            Console.WriteLine("0. (Cancel)");
            Console.Write("Your choice: ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                Console.WriteLine(keyPress.KeyChar);

                switch (keyPress.KeyChar)
                {
                    case '1':
                        return ProjectTaskStatus.ToDo;
                    case '2':
                        return ProjectTaskStatus.InProgress;
                    case '3':
                        return ProjectTaskStatus.Done;
                    case '0':
                        return null;
                }
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Invalid choice. Please select a valid option: ");
                Console.ResetColor();
            }
        }


    }
}