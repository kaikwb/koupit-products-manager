using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace koupit_products_manager.Models;

[Table("products")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("uuid", TypeName = "uuid")]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    [Column("name", TypeName = "varchar(100)")]
    public required string Name { get; set; }

    [Column("manufacturer_id")] public int ManufacturerId { get; set; }

    [ForeignKey("ManufacturerId")] public Manufacturer Manufacturer { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("part_number", TypeName = "varchar(100)")]
    public required string PartNumber { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Required] [Column("created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }

    public List<Attribute> Attributes { get; } = [];
    public List<ProductAttribute> ProductAttributes { get; } = [];
}