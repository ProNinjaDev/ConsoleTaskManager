namespace ConsoleTaskManager.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public required string ProjectId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required TaskStatus Status { get; set; }
        public int AssignedEmployeeId { get; set; } // TODO: может нужен required
    }
}