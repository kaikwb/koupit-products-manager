using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace koupit_products_manager.Models;

[Table("countries")]
public class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(name: "name", TypeName = "varchar(100)")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(2)]
    [Column(name: "code", TypeName = "char(2)")]
    public required string Code { get; set; }

    [Required] [Column("created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}