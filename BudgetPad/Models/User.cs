using System.ComponentModel.DataAnnotations;

namespace BudgetPad.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Required]
        public string Role { get; set; } = "member";
        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
