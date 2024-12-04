namespace WebApiFlight_Example.Models;

public class AppDbContext : DbContext
{
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

   public DbSet<Flight> flights { get; set; }
}
