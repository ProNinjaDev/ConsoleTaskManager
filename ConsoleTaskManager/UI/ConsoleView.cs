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
    }
}