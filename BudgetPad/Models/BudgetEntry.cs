using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null;
  }
}
