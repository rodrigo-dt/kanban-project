using Kanban_Functions.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Kanban_Functions.Services;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDBConnection"];
        var databaseName = configuration["DatabaseName"];
        
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }
    
    public IMongoCollection<BoardModel> Boards => _database.GetCollection<BoardModel>("Boards");
}