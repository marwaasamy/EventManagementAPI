using AuthenticationAPI.DTOs;
using EduJoy.BLL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO, string[] role);
        public Task<AuthDTO> LoginAsync(LoginDTO loginDTO);
        Task<AuthDTO> ConfirmEmailAsync(ConfirmEmailOtpDTO confirmEmailOtpDTO);
        public Task<ResponseDTO> ResendConfirmationEmailAsync(string email);
        public Task<string> AddToRoleAsync(AddToRoleDTO addToRoleDTO);
        public Task<ResponseDTO> ForgotPasswordAsync(string email);
        public Task<ResponseDTO> VerifyOtpAsync(VerifyOtpDTO verifyOtpDTO);
        public Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    }
}
