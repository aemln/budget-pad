using BudgetPad.Models;

namespace BudgetPad.Repositories
{
  public interface IBudgetRepository
  {
    public Task<List<BudgetEntry>> GetAll();
    public Task<BudgetEntry?> GetById(int id);
    public Task<BudgetEntry> Add(BudgetEntry budget);
    public Task<BudgetEntry?> Update(int id, BudgetEntry budget);
    public Task<BudgetEntry?> Delete(int id);
    public Task<List<BudgetEntry>> GetByUserId(int userId);
  }
}
