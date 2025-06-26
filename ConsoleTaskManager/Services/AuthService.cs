using ConsoleTaskManager.Models;
using ConsoleTaskManager.Security;
using ConsoleTaskManager.Services.Interfaces;
using ConsoleTaskManager.Storage.Interfaces;

namespace ConsoleTaskManager.Services 
{
    public class AuthService : IAuthService 
    {
        private readonly IDataStorage _dataStorage;

        public AuthService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }



        public async Task<User?> SignIn(string login, string password)
        {
            var users = await _dataStorage.LoadUsersAsync();
            var user = users.FirstOrDefault(u => u.Login.Equals(login, System.StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return null;
            }

            bool isPasswordCorrect = PasswordHasher.IsMatchPasswords(password, user.PasswordHash,
                                                                    user.PasswordSalt, user.PasswordIterations,
                                                                    user.PasswordHashAlgorithm);
            
            return isPasswordCorrect ? user : null;
        }
    }
}