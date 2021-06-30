using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSWebAPIApp.Models.Entities
{
  public class Product
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ProductId { get; set; }
    [Required, StringLength(maximumLength: 150, MinimumLength = 4)]
    public string ProductName { get; set; }
    [Required, StringLength(maximumLength: 150, MinimumLength = 4)]
    public string Description { get; set; }
    [Required, StringLength(maximumLength: 150, MinimumLength = 4)]
    public string Category { get; set; }
    [Required, Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
  }
}
