using MongoDB.Driver;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(IConfiguration configuration)
    {
        var settings = configuration.GetSection("MongoDb");

        var client = new MongoClient(settings["ConnectionString"]);
        Database = client.GetDatabase(settings["DatabaseName"]);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }
}