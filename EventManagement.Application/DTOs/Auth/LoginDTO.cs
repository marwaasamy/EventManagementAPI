using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAPI.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        // No MaxLength here on purpose: RegisterDTO allows 50, so capping login
        // at 20 would let a user register a password they could never log in
        // with. Length rules belong to Identity's password policy, not here.
        [Required]
        public string Password { get; set; }
    }
}
