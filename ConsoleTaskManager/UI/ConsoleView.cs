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
    }
}