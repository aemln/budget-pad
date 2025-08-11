using System.Security.Claims;

namespace BudgetPad.Helpers
{
  public interface IUserContextService
  {
    int? GetCurrentUserId();
    bool IsAdmin();
  }

  public class UserContextService : IUserContextService
  {
    private readonly IHttpContextAccessor _context;

    public UserContextService(IHttpContextAccessor context)
    {
      _context = context;
    }
    
    public int? GetCurrentUserId()
    {
      var userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      return int.TryParse(userId, out var id) ? id : null;
    }

    private string GetUserRole()
    {
      var role = _context.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
      return role ?? string.Empty;
    }

    public bool IsAdmin()
    {
      return GetUserRole().ToUpper() == "ADMIN";
    }
  }
}
