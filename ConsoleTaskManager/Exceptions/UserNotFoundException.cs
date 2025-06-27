namespace ConsoleTaskManager.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(object identifier)
            : base($"User with identifier '{identifier}' not found")
        {
        }

        public UserNotFoundException(object identifier, Exception innerException)
            : base($"User with identifier '{identifier}' not found", innerException)
        {
        }
    }
} 