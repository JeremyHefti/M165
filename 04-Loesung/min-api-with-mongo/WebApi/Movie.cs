using MongoDB.Bson.Serialization.Attributes;

public class Movie
{
    [BsonId]
    public string Id { get; set; } = "";

    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string Summary { get; set; } = "";
    public List<string> Actors { get; set; } = new List<string>();
}