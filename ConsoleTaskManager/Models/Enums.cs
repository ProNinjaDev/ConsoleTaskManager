namespace ConsoleTaskManager.Models
{
    public enum UserRole
    {
        Manager,
        Employee
    }

    public enum ProjectTaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public enum TaskSortField
    {
        Id,
        Name,
        Status
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}