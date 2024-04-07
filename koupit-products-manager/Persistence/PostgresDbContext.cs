using koupit_products_manager.Models;
using Microsoft.EntityFrameworkCore;

namespace koupit_products_manager.Persistence;

public class PostgresDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>()
            .HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired();
        
        modelBuilder.Entity<Manufacturer>().Navigation(e => e.Country).AutoInclude();

        base.OnModelCreating(modelBuilder);
    }
}