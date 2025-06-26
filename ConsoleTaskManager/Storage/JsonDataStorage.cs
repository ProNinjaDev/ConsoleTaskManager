using ConsoleTaskManager.Models;
using ConsoleTaskManager.Storage.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;

namespace ConsoleTaskManager.Storage
{
    public class JsonDataStorage : IDataStorage
    {
        private readonly string _usersFilePath;
        private readonly string _tasksFilePath;
        private readonly JsonSerializerOptions _serializerOptions;

        public JsonDataStorage(string usersFilePath = "users.json", string tasksFilePath = "tasks.json")
        {
            _usersFilePath = usersFilePath;
            _tasksFilePath = tasksFilePath;
            _serializerOptions = new JsonSerializerOptions 
            {
                WriteIndented = true
            };
        }

        public async Task<IEnumerable<User>> LoadUsersAsync()
        {
            if (!File.Exists(_usersFilePath))
            {
                return Enumerable.Empty<User>();
            }

            using (var stream = File.OpenRead(_usersFilePath)) 
            {
                var users = await JsonSerializer.DeserializeAsync<IEnumerable<User>>(stream);

                return users != null ? users : Enumerable.Empty<User>();
            }
        }

        public async Task<IEnumerable<ProjectTask>> LoadTasksAsync() 
        {
            if (!File.Exists(_tasksFilePath))
            {
                return Enumerable.Empty<ProjectTask>();
            }

            using (var stream = File.OpenRead(_tasksFilePath))
            {
                var tasks = await JsonSerializer.DeserializeAsync<IEnumerable<ProjectTask>>(stream);
                
                return tasks != null ? tasks : Enumerable.Empty<ProjectTask>();
            }
        }

        public async Task SaveChangesAsync(IEnumerable<User> users, IEnumerable<ProjectTask> tasks)
        {
            using (var usersStream = File.Create(_usersFilePath))
            {
                await JsonSerializer.SerializeAsync(usersStream, users, _serializerOptions);
            }

            using (var tasksStream = File.Create(_tasksFilePath))
            {
                await JsonSerializer.SerializeAsync(tasksStream, tasks, _serializerOptions);
            }
        }

    }
}
