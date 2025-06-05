using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoMovieApi.Models;

public class MongoMovieService : IMovieService
{
    private readonly IMongoCollection<Movie> _movies;

    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase("gbs");
        _movies = database.GetCollection<Movie>("movies");
    }

    public string Check()
    {
        try
        {
            var dbs = _movies.Database.Client.ListDatabaseNames().ToList();
            return $"Zugriff auf MongoDB ok. Datenbanken: {string.Join(", ", dbs)}";
        }
        catch (Exception ex)
        {
            return $"Fehler beim Zugriff auf MongoDB: {ex.Message}";
        }
    }

    public void Create(Movie movie) => _movies.InsertOne(movie);

    public IEnumerable<Movie> Get() => _movies.Find(_ => true).ToList();

    public Movie? Get(string id) => _movies.Find(m => m.Id == id).FirstOrDefault();

    public void Update(string id, Movie movie) => _movies.ReplaceOne(m => m.Id == id, movie);

    public void Remove(string id) => _movies.DeleteOne(m => m.Id == id);

    public IEnumerable<Movie> Search(string? title, int? year)
    {
        var filter = Builders<Movie>.Filter.Empty;

        if (!string.IsNullOrEmpty(title))
            filter &= Builders<Movie>.Filter.Eq(m => m.Title, title);

        if (year.HasValue)
            filter &= Builders<Movie>.Filter.Eq(m => m.Year, year.Value);

        return _movies.Find(filter).ToList();
    }

    public IEnumerable<MovieStats> GetMovieStats()
    {
        var group = new BsonDocument
        {
            { "_id", "$Year" },
            { "Count", new BsonDocument("$sum", 1) }
        };

        var pipeline = new[]
        {
            new BsonDocument("$group", group),
            new BsonDocument("$sort", new BsonDocument("_id", 1))
        };

        var result = _movies.Aggregate<BsonDocument>(pipeline).ToList();

        return result.Select(doc => new MovieStats
        {
            Year = doc["_id"].AsInt32,
            Count = doc["Count"].AsInt32
        });
    }
}