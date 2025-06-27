namespace ConsoleTaskManager.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId)
            : base($"Task with ID '{taskId}' not found")
        {
        }

        public TaskNotFoundException(int taskId, Exception innerException)
            : base($"Task with ID '{taskId}' not found", innerException)
        {
        }
    }
} 