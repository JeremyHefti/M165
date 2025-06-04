public interface IMovieService
{
    IEnumerable<Movie> Get();
    Movie Get(string id);
    void Create(Movie movie);
    public void Update(string id, Movie movie);
    public void Remove(string id);
}