using System.ComponentModel.DataAnnotations;

namespace BudgetPad.Models
{
  public class BudgetEntry
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Category { get; set; }

    [Required]
    public decimal Amount {  get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
  }
}
