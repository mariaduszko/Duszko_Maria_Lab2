using Microsoft.EntityFrameworkCore;

public class Duszko_Maria_Lab2Context(DbContextOptions<Duszko_Maria_Lab2Context> options) : DbContext(options)
{
    public DbSet<Duszko_Maria_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Duszko_Maria_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Duszko_Maria_Lab2.Models.Author> Author { get; set; } = default!;

    public DbSet<Duszko_Maria_Lab2.Models.Category> Category { get; set; } = default!;
}