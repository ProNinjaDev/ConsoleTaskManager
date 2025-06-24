using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleTaskManager.DTOs
{
    public class CreateUserDto
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}