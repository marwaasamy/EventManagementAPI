using System;
using System.Collections.Generic;
using System.Text;

namespace EduJoy.BLL.DTOs.Auth
{
    public class ConfirmEmailOtpDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
