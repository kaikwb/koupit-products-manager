using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace koupit_products_manager.Models;

[Table("product_attributes")]
public class ProductAttribute
{
    [Column("product_id")] public int ProductId { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; } = null!;

    [Column("attribute_id")] public int AttributeId { get; set; }

    [ForeignKey("AttributeId")] public Attribute Attribute { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("value", TypeName = "varchar(100)")]
    public required string Value { get; set; }

    [Required] [Column("created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}