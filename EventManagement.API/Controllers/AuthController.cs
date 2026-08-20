using AuthenticationAPI.DTOs;
using EduJoy.BLL.DTOs.Auth;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDTO, new[] { Roles.Admin });

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailOtpDTO confirmEmailOtpDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                        var result = await _authService.ConfirmEmailAsync(confirmEmailOtpDTO);

            return result.IsAuthenticated ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new ResponseDTO { Message = "Email is required", IsSuccess = false });

            var result = await _authService.ResendConfirmationEmailAsync(email);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDTO);

            return result.IsAuthenticated ? Ok(result) : BadRequest(result);
        }

     
        [HttpPost("AddToRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToRole(AddToRoleDTO addToRoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddToRoleAsync(addToRoleDTO);

            return Ok(new { message = result });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new ResponseDTO { Message = "Email is required", IsSuccess = false });

            var result = await _authService.ForgotPasswordAsync(email);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDTO verifyOtpDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.VerifyOtpAsync(verifyOtpDTO);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPasswordAsync(resetPasswordDTO);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
