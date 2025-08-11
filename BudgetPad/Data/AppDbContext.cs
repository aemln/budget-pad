using BudgetPad.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPad.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
      :base(options) { }

    public DbSet<BudgetEntry> Budgets { get; set; }
    public DbSet<User> Users { get; set; }
  }
}
