using ConsoleTaskManager.Models;
using ConsoleTaskManager.DTOs;
using System.Text;
using System.Collections;


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

        public char DisplayMenu(string title, Dictionary<char, string> options)
        {
            Console.Clear();
            DisplayHeader(title);

            foreach (var option in options)
            {
                Console.WriteLine($" {option.Key}. {option.Value}");
            }
            Console.WriteLine();
            Console.Write("Action > ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                char choice = keyPress.KeyChar;

                if (options.ContainsKey(choice))
                {
                    Console.WriteLine(choice);
                    return choice;
                }
            }
        }

        private bool BeginTableDisplay(string title, IEnumerable items, string emptyMessage)
        {
            Console.Clear();
            DisplayHeader(title);

            if (!items.Cast<object>().Any())
            {
                Console.WriteLine(emptyMessage);
                return false;
            }

            return true;
        }

        public void DisplayUsers(IEnumerable<User> users)
        {
            if (!BeginTableDisplay("All Registered Users", users, "No users found in the system"))
            {
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
            if (!BeginTableDisplay(title, tasks, "No tasks found"))
            {
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
                    DisplayInvalidChoiceMessage("Invalid ID. Please select a valid employee ID: ");
                }
            }
        }

        public ProjectTaskStatus? SelectTaskStatus()
        {
            var options = new Dictionary<char, (string Text, ProjectTaskStatus? Value)>
            {
                { '1', ("ToDo", ProjectTaskStatus.ToDo) },
                { '2', ("InProgress", ProjectTaskStatus.InProgress) },
                { '3', ("Done", ProjectTaskStatus.Done) },
                { '0', ("(Cancel)", null) }
            };

            return SelectOption<ProjectTaskStatus>("Select New Status", options);
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

        public ProjectTaskStatus? SelectStatusFilter()
        {
            var options = new Dictionary<char, (string Text, ProjectTaskStatus? Value)>
            {
                { '1', ("ToDo", ProjectTaskStatus.ToDo) },
                { '2', ("InProgress", ProjectTaskStatus.InProgress) },
                { '3', ("Done", ProjectTaskStatus.Done) },
                { '4', ("(Show All)", null) }
            };

            return SelectOption<ProjectTaskStatus>("Filter tasks by status", options);
        }

        public (TaskSortField, SortDirection) SelectSortOptions()
        {
            DisplaySubHeader("Sort tasks by");
            Console.WriteLine(" 1. ID");
            Console.WriteLine(" 2. Name");
            Console.WriteLine(" 3. Status");

            TaskSortField sortBy;
            while (true)
            {
                Console.Write("\nAction > ");
                var keyPress = Console.ReadKey(true);
                Console.WriteLine(keyPress.KeyChar);

                switch (keyPress.KeyChar)
                {
                    case '1':
                        sortBy = TaskSortField.Id;
                        break;
                    case '2':
                        sortBy = TaskSortField.Name;
                        break;
                    case '3':
                        sortBy = TaskSortField.Status;
                        break;
                    default:
                        DisplayInvalidChoiceMessage();
                        continue;
                }
                break;
            }

            DisplaySubHeader("Sort direction");
            Console.WriteLine(" 1. Ascending");
            Console.WriteLine(" 2. Descending");
            
            SortDirection sortDirection;
            while (true)
            {
                Console.Write("\nAction > ");
                var keyPress = Console.ReadKey(true);
                Console.WriteLine(keyPress.KeyChar);

                switch (keyPress.KeyChar)
                {
                    case '1':
                        sortDirection = SortDirection.Ascending;
                        break;
                    case '2':
                        sortDirection = SortDirection.Descending;
                        break;
                    default:
                        DisplayInvalidChoiceMessage();
                        continue;
                }
                break;
            }
            
            return (sortBy, sortDirection);
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

        private void DisplayInvalidChoiceMessage(string message = "Invalid choice. Please select a valid option: ")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
        }

        private T? SelectOption<T>(string subHeader, Dictionary<char, (string Text, T? Value)> options) where T : struct
        {
            DisplaySubHeader(subHeader);
            foreach (var option in options)
            {
                Console.WriteLine($" {option.Key}. {option.Value.Text}");
            }
            Console.WriteLine();
            Console.Write("Action > ");

            while (true)
            {
                var keyPress = Console.ReadKey(true);
                Console.WriteLine(keyPress.KeyChar);

                if (options.TryGetValue(keyPress.KeyChar, out var selectedOption))
                {
                    return selectedOption.Value;
                }
                
                DisplayInvalidChoiceMessage();
            }
        }

        public void WaitForAnyKey(string message = "\nPress any key to continue...")
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}