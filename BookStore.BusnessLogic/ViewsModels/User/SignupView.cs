using System.ComponentModel.DataAnnotations;

namespace BookStore.BusinessLogic.ViewModels.User
{
    public class SignupView
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name must be less than or equal to 50 characters")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name must be less than or equal to 50 characters")]
        public string Lastname { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(100, ErrorMessage = "Address must be less than or equal to 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(50, ErrorMessage = "City must be less than or equal to 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(50, ErrorMessage = "State must be less than or equal to 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Password must be less than or equal to 50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
