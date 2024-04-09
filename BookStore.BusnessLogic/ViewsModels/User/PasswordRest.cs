using System.ComponentModel.DataAnnotations;

public class PasswordReset
{
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword { get; set; }


}
