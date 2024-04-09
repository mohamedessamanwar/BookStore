using System.ComponentModel.DataAnnotations;

namespace BookStore.BusnessLogic.ViewsModels.User
{
    public class ForgetPassward
    {
        [EmailAddress(ErrorMessage = "Enter valid Email")]
        [Required]
        public string Email { get; set; }
    }


}
