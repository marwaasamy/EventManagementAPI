using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required] [MaxLength(50)]

        public string UserName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; } 

    }
}
