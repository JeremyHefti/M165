using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Movie
{
    [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)] wenn automatisch eine Id erzeugt werden soll
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string Summary { get; set; } = "";
    public string[] Actors { get; set; } = Array.Empty<string>();
    //public List<string> Actors { get; set; } = new List<string>();
    
}