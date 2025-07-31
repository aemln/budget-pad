using BudgetPad.Data;
using BudgetPad.Models;
using BudgetPad.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BudgetPad.Tests
{
  public class BudgetRepositoryTest
  {
    private readonly BudgetRepository _repository;
    private readonly AppDbContext _context;
    public BudgetRepositoryTest()
    {
      var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
      _context = new AppDbContext(options);
      _repository = new BudgetRepository(_context);

      // seed data
      var budget = new BudgetEntry { Category = "Finance", Amount = 50 };
      _context.Budgets.AddAsync(budget);
    }

    [Fact]
    public async Task AddBudget_ShouldInsertNewRecord()
    {
      var expected = new BudgetEntry { Category = "HR", Amount = 40 };
      var actual = await _repository.Add(expected);
      Assert.Equal("HR", actual.Category);
      Assert.NotEmpty(await _context.Budgets.ToListAsync());
    }

    [Fact]
    public async Task GetBudgetById_ShouldReturnCorrectly()
    {
      var expected = await _context.Budgets.FindAsync(1);
      var actual = await _repository.GetById(expected.Id);

      Assert.NotNull(actual);
      Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task UpdateBudget_ShouldModifyRecord()
    {
      var expected = await _context.Budgets.FindAsync(1);
      expected.Amount = 60;
      expected.Category = "HR";
      var actual = await _repository.Update(expected.Id, expected);
      
      Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DeleteBudget_ShouldDeleteRecord()
    {
      var expected = new BudgetEntry { Category = "HR", Amount = 65 };
      await _context.AddAsync(expected);

      var actual = await _repository.Delete(expected.Id);
      Assert.Equal(expected, actual);

      var deletedBudget = await _context.Budgets.FindAsync(expected.Id);
      Assert.Null(deletedBudget);
    }
  }
}
