using MongoDB.Driver;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);
var databaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(databaseConfigSection);
builder.Services.AddSingleton<IMovieService, MongoMovieService>(); 

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/check", (IMovieService service) =>
{
    try
    {
        return Results.Ok(service.Check());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/movies", (IMovieService service) =>
{
    try
    {
        return Results.Ok(service.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/movies/{id}", (IMovieService service, string id) =>
{
    try
    {
        var movie = service.Get(id);
        return movie is null ? Results.NotFound() : Results.Ok(movie);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/movies", (IMovieService service, Movie movie) =>
{
    try
    {
        service.Create(movie);
        return Results.Ok(movie);
    }
    catch (Exception ex)
    {
        return Results.Conflict(ex.Message);
    }
});

app.MapPut("/api/movies/{id}", (IMovieService service, string id, Movie movie) =>
{
    try
    {
        var existing = service.Get(id);
        if (existing is null)
        {
            return Results.NotFound();
        }

        movie.Id = id;
        service.Update(id, movie);
        return Results.Ok(movie);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/movies/{id}", (IMovieService service, string id) =>
{
    try
    {
        var existing = service.Get(id);
        if (existing is null)
        {
            return Results.NotFound();
        }

        service.Remove(id);
        return Results.Ok($"Movie mit Id {id} gelöscht.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/movies/search", (IMovieService service, string? title, int? year) =>
{
    try
    {
        return Results.Ok(service.Search(title, year));
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/movies/stats", (IMovieService service) =>
{
    try
    {
        return Results.Ok(service.GetMovieStats());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
