using koupit_products_manager.Models;
using Microsoft.EntityFrameworkCore;
using Attribute = koupit_products_manager.Models.Attribute;

namespace koupit_products_manager.Persistence;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Product> Products { get; set; }

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

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Manufacturer)
            .WithMany()
            .HasForeignKey(p => p.ManufacturerId)
            .HasPrincipalKey(m => m.Id)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Attributes)
            .WithMany(a => a.Products)
            .UsingEntity<ProductAttribute>(
                l => l.HasOne(pa => pa.Attribute).WithMany(a => a.ProductAttributes).HasForeignKey(pa => pa.AttributeId),
                r => r.HasOne(pa => pa.Product).WithMany(p => p.ProductAttributes).HasForeignKey(pa => pa.ProductId));

        modelBuilder.Entity<Product>().Navigation(p => p.Manufacturer).AutoInclude();
        modelBuilder.Entity<Product>().Navigation(p => p.ProductAttributes).AutoInclude();

        modelBuilder.Entity<ProductAttribute>().Navigation(pa => pa.Product).AutoInclude();
        modelBuilder.Entity<ProductAttribute>().Navigation(pa => pa.Attribute).AutoInclude();

        base.OnModelCreating(modelBuilder);
    }
}