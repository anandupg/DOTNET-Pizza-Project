using MongoDB.Driver;
using Microsoft.Extensions.Options;
using PizzaApi.Models;

namespace PizzaApi.Services;

public class MongoSettings
{
    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "";
    public string PizzasCollection { get; set; } = "Pizzas";
    public string UsersCollection { get; set; } = "Users";
}

public class PizzaService
{
    private readonly IMongoCollection<Pizza> _pizzas;

    public PizzaService(IOptions<MongoSettings> mongoSettings)
    {
        var settings = mongoSettings.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _pizzas = db.GetCollection<Pizza>(settings.PizzasCollection);
    }

    public async Task<List<Pizza>> GetAsync() =>
        await _pizzas.Find(_ => true).ToListAsync();

    public async Task<Pizza> CreateAsync(Pizza pizza)
    {
        await _pizzas.InsertOneAsync(pizza);
        return pizza;
    }

    // Add more methods as needed for full CRUD
    public async Task<Pizza?> GetAsync(string id) =>
        await _pizzas.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, Pizza updatedPizza) =>
        await _pizzas.ReplaceOneAsync(x => x.Id == id, updatedPizza);

    public async Task RemoveAsync(string id) =>
        await _pizzas.DeleteOneAsync(x => x.Id == id);
}
