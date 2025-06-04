var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

// MovieService wird als Singleton registriert.
// Er kann Dank DependencyInjection in allen app.Map...-Methoden verwendet werden.
builder.Services.AddSingleton<IMovieService, MongoMovieService>();
var app = builder.Build();

app.MapGet("/", () => "Movies API");

// Get all Movies
app.MapGet("api/movies", (IMovieService movieService) =>
{
    var movies = movieService.Get();
    return Results.Ok(movies);
});

// Get Movie by id
app.MapGet("api/movies/{id}", (IMovieService movieService, string id) =>
{
    var movie = movieService.Get(id);
    return movie != null
        ? Results.Ok(movie)
        : Results.NotFound();
});

// Insert Movie
app.MapPost("/api/movies", (IMovieService movieService, Movie movie) =>
{
    movieService.Create(movie);
    return Results.Ok(movie);
});

// Update Movie
app.MapPut("/api/movies/{id}", (IMovieService movieService, string id, Movie movie) =>
{
    var existingMovie = movieService.Get(id);
    if (existingMovie == null)
    {
        return Results.NotFound();
    }

    movieService.Update(id, movie);
    return Results.Ok(movie);
});

// Delete Movie
app.MapDelete("api/movies/{id}", (IMovieService movieService, string id) =>
{
    var movie = movieService.Get(id);
    if(movie == null)
    {
        return Results.NotFound();
    }
   
    movieService.Remove(id);
    return Results.Ok();
});

app.Run();