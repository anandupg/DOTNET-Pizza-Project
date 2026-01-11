using MongoDB.Driver;
using Microsoft.Extensions.Options;
using PizzaApi.Models;

namespace PizzaApi.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IOptions<MongoSettings> mongoSettings)
    {
        var settings = mongoSettings.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _users = db.GetCollection<User>(settings.UsersCollection);
    }

    public async Task<User?> LoginAsync(string username, string password) =>
        await _users.Find(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();

    public async Task CreateAsync(User user)
    {
        // Check if exists first
        var existing = await _users.Find(x => x.Username == user.Username).FirstOrDefaultAsync();
        if (existing == null)
        {
            await _users.InsertOneAsync(user);
        }
    }
}
