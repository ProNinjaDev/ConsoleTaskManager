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

        public (string Login, string Password) GetLoginCredentials()
        {
            return GetCredentials("Log In", "Login", "Password");
        }

        public (string Login, string Password) GetNewUserDetails()
        {
            return GetCredentials("Register New Employee", "New employee's login", "New employee's password");
        }

        private (string Login, string Password) GetCredentials(string title, string loginPrompt, string passwordPrompt)
        {
            Console.Clear();
            DisplayHeader(title);

            Console.Write($"{loginPrompt} > ");
            string login = Console.ReadLine() ?? string.Empty;

            Console.Write($"{passwordPrompt} > ");
            string password = ReadPassword();
            Console.WriteLine();

            return (login, password);
        }

        public char DisplayManagerMenu()
        {
            Console.Clear();
            DisplayHeader("Manager's Menu");
            Console.WriteLine(" 1. Create a new task");
            Console.WriteLine(" 2. Register a new employee");
            Console.WriteLine(" 3. View all tasks");
            Console.WriteLine(" 4. View all users");
            Console.WriteLine(" 5. View task activity log");
            Console.WriteLine(" 0. Log out");
            Console.WriteLine();
            Console.Write("Action > ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                char choice = keyPress.KeyChar;

                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == '5' || choice == '0')
                {
                    Console.WriteLine(choice);
                    return choice;
                }
            }
        }

        public char DisplayEmployeeMenu()
        {
            Console.Clear();
            DisplayHeader("Employee's Menu");
            Console.WriteLine(" 1. View my assigned tasks");
            Console.WriteLine(" 2. Change a task's status");
            Console.WriteLine(" 0. Log out");
            Console.WriteLine();
            Console.Write("Action > ");

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
            DisplayHeader("All Registered Users");

            if (!users.Any())
            {
                Console.WriteLine("No users found in the system");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"LOGIN",-20} {"ROLE",-10}");
            Console.WriteLine(new string('-', 37));
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id,-5} {user.Login,-20} {user.Role,-10}");
            }
            Console.WriteLine(new string('-', 37));
        }

        public void DisplayTasks(IEnumerable<ProjectTask> tasks, string title)
        {
            Console.Clear();
            DisplayHeader(title);

            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"NAME",-25} {"STATUS",-12} {"ASSIGNED TO",-12}");
            Console.WriteLine(new string('-', 60));
            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id,-5} {task.Name,-25} {task.Status,-12} {"Employee " + task.AssignedEmployeeId,-12}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public CreateTaskDto GetNewTaskDetails()
        {
            Console.Clear();
            DisplayHeader("Enter Task Details");

            Console.Write("Name > ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description > ");
            string description = Console.ReadLine() ?? string.Empty;
            Console.Write("Project ID > ");
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
            DisplayHeader("Assign Task to Employee");
            
            Console.WriteLine($"{"ID",-5} {"LOGIN",-20}");
            Console.WriteLine(new string('-', 27));
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id,-5} {employee.Login,-20}");
            }
            Console.WriteLine(new string('-', 27));
            Console.WriteLine();
            Console.Write("Enter Employee ID (or 0 to cancel) > ");

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
            DisplaySubHeader("Select New Status");
            Console.WriteLine(" 1. ToDo");
            Console.WriteLine(" 2. InProgress");
            Console.WriteLine(" 3. Done");
            Console.WriteLine(" 0. (Cancel)");
            Console.WriteLine();
            Console.Write("Action > ");

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

        public void DisplayLogs(IEnumerable<string> logs)
        {
            Console.Clear();
            DisplayHeader("Task Activity Log");

            if (!logs.Any())
            {
                Console.WriteLine("No activity has been logged yet");
                return;
            }

            foreach (var logEntry in logs)
            {
                Console.WriteLine(logEntry);
            }
        }

        private void DisplayHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var formattedTitle = $"== {title.ToUpper()} ==";
            Console.WriteLine(formattedTitle);
            Console.WriteLine(new string('=', formattedTitle.Length));
            Console.ResetColor();
            Console.WriteLine();
        }

        private void DisplaySubHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n-- {title} --");
            Console.ResetColor();
        }
    }
}