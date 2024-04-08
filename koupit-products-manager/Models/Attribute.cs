using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace koupit_products_manager.Models;

public enum AttributeDataType
{
    [PgName("string")] String,
    [PgName("integer")] Integer,
    [PgName("float")] Float,
    [PgName("boolean")] Boolean,
}

[Table("attributes")]
public class Attribute
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

    [Required]
    [MaxLength(100)]
    [Column("pretty_name", TypeName = "varchar(100)")]
    public required string PrettyName { get; set; }

    [MaxLength(100)]
    [Column("unit", TypeName = "varchar(100)")]
    public string? Unit { get; set; }

    [Required]
    [Column("data_type", TypeName = "attribute_data_type")]
    public AttributeDataType DataType { get; set; }

    [Required] [Column("created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}