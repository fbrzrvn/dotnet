using Microsoft.EntityFrameworkCore;

namespace MoviesWebApp.Data;

public class MoviesWebAppContext : DbContext
{
    public MoviesWebAppContext(DbContextOptions<MoviesWebAppContext> options)
        : base(options)
    {
    }

    public DbSet<MoviesWebApp.Models.Movie> Movie { get; set; } = default!;
}
