using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.Data;

namespace DatingApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<(bool Success, string ErrorMessage)> Register(string username, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null)
            {
                return (false, "An account with this email already exists.");
            }

            var user = new ApplicationUser { UserName = username, Email = username };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return (true, null);
            }
            else
            {
                var error = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, error);
            }
        }
    }
}
