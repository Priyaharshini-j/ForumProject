using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models
{
    public class UsersModel
    {
        [DisplayName("UserId")]
        public int Id { get; set; }
        [DisplayName("User Name")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [DisplayName("Password")]
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one non-alphanumeric character.")]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Security Question")]
        public string securityQn { get; set; }
        [DisplayName("Secuirty Answer")]
        public string securityAns { get; set; }

    }
}
