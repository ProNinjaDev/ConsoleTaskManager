namespace ConsoleTaskManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required int PasswordIterations { get; set; }
        public required string PasswordHashAlgorithm { get; set; }
        public required UserRole Role { get; set; }
    }
}