using MongoDB.Driver;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);
var databaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(databaseConfigSection);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/check", (IOptions<DatabaseSettings> options) =>
{
    try
    {
        var connectionString = options.Value.ConnectionString;

        // Verbindung aufbauen
        var client = new MongoClient(connectionString);

        // Datenbanken abrufen
        var dbList = client.ListDatabaseNames().ToList();

        // Ergebnis als String zurückgeben
        return Results.Ok($"Zugriff auf MongoDB ok. Datenbanken: {string.Join(", ", dbList)}");
    }
    catch (Exception ex)
    {
        // Fehlerbehandlung
        return Results.Problem($"Fehler beim Zugriff auf MongoDB: {ex.Message}");
    }
});

app.Run();
