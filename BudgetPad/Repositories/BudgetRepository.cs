using BudgetPad.Data;
using BudgetPad.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPad.Repositories
{
  public class BudgetRepository : IBudgetRepository
  {
    private readonly AppDbContext _context;
    public BudgetRepository(AppDbContext context)
    {
      _context = context;
    }
    public async Task<List<BudgetEntry>> GetAll()
    {
      return await _context.Budgets.ToListAsync();
    }

    public async Task<BudgetEntry?> GetById(int id)
    {
      return await _context.Budgets.FindAsync(id);
    }
    public async Task<BudgetEntry> Add(BudgetEntry budget)
    {
      _context.Budgets.Add(budget);
      await _context.SaveChangesAsync();
      return budget;
    }

    public async Task<BudgetEntry?> Update(int id, BudgetEntry budget)
    {
      var newBudget = await _context.Budgets.FindAsync(id);
      if (newBudget != null) {
        newBudget.Category = budget.Category;
        newBudget.Amount = budget.Amount;
        await _context.SaveChangesAsync();
      }
      return newBudget;
    }
    public async Task<BudgetEntry?> Delete(int id)
    {
      var budget = await _context.Budgets.FindAsync(id);
      if (budget != null){
        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();
      }
      return budget;
    }

    public async Task<List<BudgetEntry>> GetByUserId(int userId)
    {
      var budgets = await _context.Budgets
        .Where(b => b.UserId == userId)
        .Include(b => b.User)
        .ToListAsync();
      return budgets;
    }
  }
}
