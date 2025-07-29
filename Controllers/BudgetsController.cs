using BudgetPad.Data;
using BudgetPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetPad.Controllers
{
  [Route("api/budgets")]
  [ApiController]
  public class BudgetsController : ControllerBase
  {
    private readonly AppDbContext _context;

    public BudgetsController(AppDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetEntry>>> Get()
    {
      return await _context.Budgets.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetEntry>> Get(int id)
    {
      var budget = await _context.Budgets.FindAsync(id);

      if (budget == null) return NotFound();
      return Ok(budget);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetEntry>> Post(BudgetEntry budget)
    {
      budget.CreatedDate = DateTime.UtcNow;
      _context.Budgets.Add(budget);
      await _context.SaveChangesAsync();

      return CreatedAtAction(
        nameof(Get),
        new { id = budget.Id },
        budget);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetEntry>> Put(int id, BudgetEntry budget)
    {
      var currentBudget = await _context.Budgets.FindAsync(id);
      if (currentBudget == null) return NotFound();
      if (id != budget.Id) return BadRequest();

      currentBudget.Category = budget.Category;
      currentBudget.Amount = budget.Amount;
      await _context.SaveChangesAsync();
      return Ok(currentBudget);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var budget = await _context.Budgets.FindAsync(id);
      if (budget == null) return NotFound();

      _context.Budgets.Remove(budget);
      await _context.SaveChangesAsync();

      return Ok(budget);
    }
  }
}
