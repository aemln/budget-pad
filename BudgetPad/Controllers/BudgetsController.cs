using BudgetPad.Data;
using BudgetPad.Helpers;
using BudgetPad.Models;
using BudgetPad.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetPad.Controllers
{
  [Authorize]
  [Route("api/budgets")]
  [ApiController]
  public class BudgetsController : ControllerBase
  {
    private readonly IBudgetRepository _repository;
    private readonly IUserContextService _userContext;

    public BudgetsController(IBudgetRepository repository, IUserContextService userContext)
    {
      _repository = repository;
      _userContext = userContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetResponseDto>>> Get()
    {
      IEnumerable<BudgetEntry> budgets;
      if (_userContext.IsAdmin())
      {
        budgets = await _repository.GetAll();
      }
      else
      {
        var userId = _userContext.GetCurrentUserId();
        budgets = await _repository.GetByUserId(userId!.Value);
      }

      return Ok(budgets.Select(b => new BudgetResponseDto
      {
        Id = b.Id,
        Category = b.Category,
        Amount = b.Amount,
        CreatedDate = b.CreatedDate,
        UserId = b.UserId,
      }).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetResponseDto>> Get(int id)
    {
      var budget = await _repository.GetById(id);

      if (budget == null) return NotFound();
      if (!isBudgetOwner(budget)) return Forbid();

      return Ok(new BudgetResponseDto
      {
        Id = id,
        Category = budget.Category,
        Amount = budget.Amount,
        CreatedDate = budget.CreatedDate,
        UserId = budget.UserId,
      });
    }

    [HttpPost]
    public async Task<ActionResult<BudgetResponseDto>> Post(BudgetRequestDto budgetDto)
    {
      var newBudget = new BudgetEntry
      {
        Category = budgetDto.Category,
        Amount = budgetDto.Amount,
        CreatedDate = DateTime.UtcNow,
        UserId = _userContext.GetCurrentUserId()!.Value,
      };

      newBudget = await _repository.Add(newBudget);

      return CreatedAtAction(
        nameof(Get),
        new { id = newBudget.Id },
        new BudgetResponseDto
        {
          Id = newBudget.Id,
          Category = newBudget.Category,
          Amount = newBudget.Amount,
          CreatedDate = newBudget.CreatedDate,
          UserId = newBudget.UserId,
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetResponseDto>> Put(int id, BudgetRequestDto budgetDto)
    {
      var budget = await _repository.GetById(id);
      if (budget == null) return NotFound();
      if (!isBudgetOwner(budget)) return Forbid();

      budget.Category = budgetDto.Category;
      budget.Amount = budgetDto.Amount;
      var updatedBudget = await _repository.Update(id, budget);

      return Ok(new BudgetResponseDto
      {
        Id = updatedBudget.Id,
        Category = updatedBudget.Category,
        Amount = updatedBudget.Amount,
        CreatedDate = updatedBudget.CreatedDate,
        UserId = updatedBudget.UserId,
      });
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<BudgetResponseDto>> Delete(int id)
    {
      var budget = await _repository.Delete(id);
      if (budget == null) return NotFound();
      return Ok(new BudgetResponseDto
      {
        Id = id,
        Category = budget.Category,
        Amount = budget.Amount,
        CreatedDate = budget.CreatedDate,
        UserId = budget.UserId,
      });
    }

    private Boolean isBudgetOwner(BudgetEntry budget)
    {
      return _userContext.IsAdmin()
        || budget.UserId == _userContext.GetCurrentUserId();
    }
  }
}
