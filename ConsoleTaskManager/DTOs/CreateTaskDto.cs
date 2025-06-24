using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleTaskManager.DTOs
{
    public class CreateTaskDto
    {
        public required string ProjectId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}