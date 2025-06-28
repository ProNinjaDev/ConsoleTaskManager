using ConsoleTaskManager.Exceptions;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Security;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage.Interfaces;
using ConsoleTaskManager.Utils;

namespace ConsoleTaskManager.Services 
{
    public class UserService : IUserService 
    {
        private readonly IDataStorage _dataStorage;

        public UserService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<User> CreateUserAsync(string login, string password, UserRole role = UserRole.Employee) 
        {
            var users = (await _dataStorage.LoadUsersAsync()).ToList();

            if (users.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateLoginException(login);
            }

            var passwordData = PasswordHasher.GenerateHashedPassword(password);

            int newId = IdGenerator.GenerateNextId(users, u => u.Id);

            var newUser = new User
            {
                Id = newId,
                Login = login,
                PasswordHash = passwordData.Hash,
                PasswordSalt = passwordData.Salt,
                PasswordIterations = passwordData.Iterations,
                PasswordHashAlgorithm = passwordData.Algorithm,
                Role = role
            };

            users.Add(newUser);
            await _dataStorage.SaveUsersAsync(users);

            return newUser;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _dataStorage.LoadUsersAsync();
            return users;
        }

        public async Task<IEnumerable<User>> GetAllEmployeesAsync()
        {
            var users = await _dataStorage.LoadUsersAsync();
            return users.Where(u => u.Role == UserRole.Employee);
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            var users = await _dataStorage.LoadUsersAsync();
            return users.FirstOrDefault(u => u.Login.Equals(login, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var users = await _dataStorage.LoadUsersAsync();
            return users.FirstOrDefault(u => u.Id == userId);
        }
    }
}