using koupit_products_manager.Models;
using Microsoft.EntityFrameworkCore;
using Attribute = koupit_products_manager.Models.Attribute;

namespace koupit_products_manager.Persistence;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Attribute> Attributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>()
            .HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired();

        modelBuilder.Entity<Manufacturer>().Navigation(e => e.Country).AutoInclude();

        modelBuilder.HasPostgresEnum<AttributeDataType>("attribute_data_type");

        base.OnModelCreating(modelBuilder);
    }
}