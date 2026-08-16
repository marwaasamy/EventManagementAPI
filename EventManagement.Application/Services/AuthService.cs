using AuthenticationAPI.DTOs;
using EduJoy.BLL.DTOs.Auth;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly JWT _jwt;
        private readonly IEmailService _emailSender;
        private readonly IMemoryCache _memoryCache;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IEmailService emailSender, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _RoleManager = roleManager;
            _jwt = jwt.Value;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO, string[] role)
        {
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new ResponseDTO
                {
                    Message = "this Email is already registered"
                };
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.UserName
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));

                return new ResponseDTO { Message = errors };
            }

            await _userManager.AddToRolesAsync(user, role);

            // send email confirmation OTP here
            await SendEmailConfirmationOtpAsync(user);

            return new ResponseDTO
            {
                Message = "User Registered successfully. Please check your email for the confirmation code.",
                IsSuccess = true
            };
        }

        // Shared by RegisterAsync and ResendConfirmationEmailAsync.
        // Caches both the real Identity confirmation token (needed to call
        // ConfirmEmailAsync later) and a short OTP (what the user actually receives).
        private async Task SendEmailConfirmationOtpAsync(ApplicationUser user)
        {
            var identityToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var otp = new Random().Next(100000, 999999).ToString();

            _memoryCache.Set($"EmailConfirmOtp_{user.Email}", otp, TimeSpan.FromMinutes(15));
            _memoryCache.Set($"EmailConfirmToken_{user.Email}", identityToken, TimeSpan.FromMinutes(15));

            var emailBody = $" <h2>Email Confirmation</h2>" +
                $"<p> Hello {System.Net.WebUtility.HtmlEncode(user.FullName)},</p>" +
                $"<p>Use the code below to confirm your email address:</p>" +
                $"<h3 style='color: #007bff;'>{otp}</h3>" +
                $"<p>This code will expire in 15 minutes.</p>";

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);
        }

        public async Task<AuthDTO> ConfirmEmailAsync(ConfirmEmailOtpDTO confirmEmailOtpDTO)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailOtpDTO.Email);
            if (user == null)
            {
                return new AuthDTO { Message = "User not found" };
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return new AuthDTO { Message = "Email is already confirmed. You can log in." };
            }

            if (!_memoryCache.TryGetValue($"EmailConfirmOtp_{user.Email}", out string cachedOtp))
            {
                return new AuthDTO { Message = "Code expired or not found. Please request a new one." };
            }

            if (cachedOtp != confirmEmailOtpDTO.Otp)
            {
                return new AuthDTO { Message = "Invalid code" };
            }

            if (!_memoryCache.TryGetValue($"EmailConfirmToken_{user.Email}", out string identityToken))
            {
                return new AuthDTO { Message = "Confirmation session expired. Please request a new code." };
            }

            var result = await _userManager.ConfirmEmailAsync(user, identityToken);

            if (result.Succeeded)
            {
                _memoryCache.Remove($"EmailConfirmOtp_{user.Email}");
                _memoryCache.Remove($"EmailConfirmToken_{user.Email}");

                var roles = await _userManager.GetRolesAsync(user);
                var token = CreateJWTToken(user, roles);
                return new AuthDTO
                {
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Email confirmed successfully. You can now log in.",
                    ExpiresOn = token.ValidTo,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    UserName = user.UserName ?? string.Empty,
                    Roles = roles.ToList()
                };
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new AuthDTO { Message = errors };
            }
        }


        public async Task<ResponseDTO> ResendConfirmationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDTO { Message = "User not found", IsSuccess = false };
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return new ResponseDTO { Message = "Email is already confirmed. You can log in.", IsSuccess = false };
            }

            await SendEmailConfirmationOtpAsync(user);

            return new ResponseDTO
            {
                Message = "Confirmation code resent. Please check your email to confirm your account."
                ,
                IsSuccess = true,
            };
        }


        public async Task<AuthDTO> LoginAsync(LoginDTO loginDTO)
        {
            var finduser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (finduser == null || !await _userManager.CheckPasswordAsync(finduser, loginDTO.Password))
            {
                return new AuthDTO { Message = "Email or Password is incorrect" };
            }
            if (!await _userManager.IsEmailConfirmedAsync(finduser))
            {
                return new AuthDTO { Message = "Email is not confirmed. Please check your email to confirm your account." };
            }

            var roles = await _userManager.GetRolesAsync(finduser);
            var token = CreateJWTToken(finduser, roles);

            return new AuthDTO
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo,
                Email = finduser.Email ?? string.Empty,
                FullName = finduser.FullName,
                UserName = finduser.UserName ?? string.Empty,
                Roles = roles.ToList()
            };

        }

      

        public async Task<string> AddToRoleAsync(AddToRoleDTO addToRoleDTO)
        {
            var user = await _userManager.FindByIdAsync(addToRoleDTO.UserId);
            if (user == null || !await _RoleManager.RoleExistsAsync(addToRoleDTO.RoleName))
            {
                return " Invalid user ID or Role";
            }

            if (await _userManager.IsInRoleAsync(user, addToRoleDTO.RoleName))
            {
                return "User Already assigned to this role";
            }
            var result = await _userManager.AddToRoleAsync(user, addToRoleDTO.RoleName);

            return result.Succeeded ? "User added to role successfully" : "Failed to add user to role";

        }


        private JwtSecurityToken CreateJWTToken(ApplicationUser user, IList<string> userRoles)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim (ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim (ClaimTypes.NameIdentifier,user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _jwt.IssuerIP,
                audience: _jwt.AudienceIP,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationDays),
                signingCredentials: signingCredentials
            );
            return token;
        }

        public async Task<ResponseDTO> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDTO
                {
                    Message = "User not found",
                    IsSuccess = false
                };
            }

            var otp = new Random().Next(100000, 999999).ToString();

            _memoryCache.Set($"OTP_{user.Email}", otp, TimeSpan.FromMinutes(15));

            var emailBody = $@"
                          <h2>Password Reset Request</h2>
                          <p>Hello {System.Net.WebUtility.HtmlEncode(user.FullName)},</p>
                          <p>You requested to reset your password. Use the OTP below to proceed:</p>
                          <h3 style='color: #007bff;'>{otp}</h3>
                          <p>This OTP will expire in 15 minutes.</p>
                          <p>If you did not request this, please ignore this email.</p>";

            await _emailSender.SendEmailAsync(user.Email, "Password Reset OTP", emailBody);

            return new ResponseDTO
            {
                Message = "OTP sent to your email. Please check your inbox.",
                IsSuccess = true
            };
        }

        public async Task<ResponseDTO> VerifyOtpAsync(VerifyOtpDTO verifyOtpDTO)
        {
            var user = await _userManager.FindByEmailAsync(verifyOtpDTO.Email);
            if (user == null)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "User Not Found"
                };
            }

            if (!_memoryCache.TryGetValue($"OTP_{user.Email}", out string CachedOtp))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "OTP expired or not found. Please request a new OTP."
                };
            }

            if (CachedOtp != verifyOtpDTO.Otp)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid OTP"
                };
            }

            _memoryCache.Set($"Verified_{user.Email}", true, TimeSpan.FromMinutes(10));
            _memoryCache.Remove($"OTP_{user.Email}");


            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "OTP verified. You may now reset your password"
            };
        }

        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "User Not Found"
                };
            }

            if (!_memoryCache.TryGetValue($"Verified_{user.Email}", out bool isVerified) || !isVerified)
            {
                return new ResponseDTO
                {
                    Message = "Please verify your OTP first",
                    IsSuccess = false
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            _memoryCache.Remove($"Verified_{user.Email}");

            return new ResponseDTO
            {
                Message = "Password has been reset successfully",
                IsSuccess = true
            };

        }
    }
}
