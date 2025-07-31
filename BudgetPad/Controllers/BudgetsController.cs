using BudgetPad.Data;
using BudgetPad.Models;
using BudgetPad.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetPad.Controllers
{
  [Route("api/budgets")]
  [ApiController]
  public class BudgetsController : ControllerBase
  {
    private readonly IBudgetRepository _repository;

    public BudgetsController(IBudgetRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetEntry>>> Get()
    {
      var budgets = await _repository.GetAll();
      return Ok(budgets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetEntry>> Get(int id)
    {
      var budgets = await _repository.GetById(id);

      if (budgets == null) return NotFound();
      return Ok(budgets);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetEntry>> Post(BudgetEntry budget)
    {
      var newBudget = await _repository.Add(budget); 
      return CreatedAtAction(
        nameof(Get),
        new { id = newBudget.Id },
        newBudget);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetEntry>> Put(int id, BudgetEntry budget)
    {
      if (id != budget.Id) return BadRequest();
      var updatedBudget = await _repository.Update(id, budget);
      if(updatedBudget == null) return NotFound();
      return Ok(updatedBudget);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var budget = await _repository.Delete(id);
      if (budget == null) return NotFound();
      return Ok(budget);
    }
  }
}
