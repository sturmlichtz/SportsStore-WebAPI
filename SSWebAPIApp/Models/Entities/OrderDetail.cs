using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSWebAPIApp.Models.Entities
{
  public class OrderDetail
  {
    [Key]
    [Column(Order = 1)]
    public long OrderId { get; set; }
    [Key]
    [Column(Order = 2)]
    public long ProductId { get; set; }
    [Required, StringLength(maximumLength: 150, MinimumLength = 4)]
    public string ProductName { get; set; }
    [Required, Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
    [Required]
    public int Count { get; set; }
  }
}
