using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace koupit_products_manager.Models;

[Table("manufacturers")]
public class Manufacturer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column(name: "uuid", TypeName = "uuid")]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    [Column(name: "name", TypeName = "varchar(100)")]
    public required string Name { get; set; }

    [Display(Name = "Country")]
    [Column("country_id")]
    public int CountryId { get; set; }
    
    [ForeignKey("CountryId")]
    public Country Country { get; set; } = null!;

    [Required] [Column("created_at")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}