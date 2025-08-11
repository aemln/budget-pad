using System.ComponentModel.DataAnnotations;

namespace BudgetPad.Models
{
  public class BudgetResponseDto
  {
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
  }
}
