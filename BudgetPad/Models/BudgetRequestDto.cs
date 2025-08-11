using System.ComponentModel.DataAnnotations;

namespace BudgetPad.Models
{
  public class BudgetRequestDto
  {
    public string Category { get; set; }
    public decimal Amount { get; set; }
  }
}
