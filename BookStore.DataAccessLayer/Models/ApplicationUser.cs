using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace BookStore.DataAccessLayer.Models

{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required, MaxLength(100)]
        public string Address { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, MaxLength(50)]
        public string State { get; set; }


    }
}
