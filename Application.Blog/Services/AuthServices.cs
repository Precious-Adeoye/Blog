using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Blog.DTOs;
using Application.Blog.Iservice;
using BlogApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Application.Blog.DTOs.Response;

namespace Application.Blog.Services
{
    public class AuthServices : IAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthServices(RoleManager<IdentityRole> rolemanger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _roleManager = rolemanger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<GeneralResponse> RegisterAsync(RegisterDto Dto)
        {
            if(Dto == null)
            {
                return new GeneralResponse(false, "Cannot be empty");
            }

            var user = new ApplicationUser
            {
                UserName = Dto.UserName,
                Email = Dto.Email,
                fullname = Dto.FullName,
            };

            var existingUser = await _userManager.FindByNameAsync(Dto.UserName);
            if (existingUser != null)
            {
                return new GeneralResponse(false, "Username already exist");
            }

            var result = await _userManager.CreateAsync(user!, Dto.Password);
            if (!result.Succeeded)
                return new GeneralResponse(false, "An error occured");
          
            string role = "Guest";
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            await _userManager.GetRolesAsync(user);
            return new GeneralResponse(true, "User successfully created");
        }

        public async Task<LogInResponse> LoginAsync(LoginDto Dto)
        {
            if (Dto == null)
            {
                return  new LogInResponse(false, null, "It cannot be empty");
            }

            var getUser = await _userManager.FindByEmailAsync(Dto.Email);
            if (getUser == null)
            {
                return new LogInResponse(false, null, "User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(getUser, Dto.Password, false, false);
            if (!result.Succeeded)
            {
                var errors = new List<string>();
                if (result.IsLockedOut) errors.Add("User is locked out.");
                if (result.IsNotAllowed) errors.Add("User is not allowed to sign in.");
                if (result.RequiresTwoFactor) errors.Add("Two-factor authentication required.");
                if (!result.Succeeded && errors.Count == 0) errors.Add("Invalid login attempt.");
                return new LogInResponse(false, null, string.Join(" ", errors));
               // return new LogInResponse(false, null, "Invalid login attempt");
            }

            var roles = await _userManager.GetRolesAsync(getUser);
            var session = new UserSession(getUser.Id, getUser.fullname, getUser.Email, roles.First());

            if (session == null)
            {
                throw new Exception("Invalid email or Password. "); 
            }

            string token = GenerateToken(session);
            return new LogInResponse(true, token, "Login successful");
        }

        public async Task<GeneralResponse> LogoutAsync()
        {
          await _signInManager.SignOutAsync();
            return new GeneralResponse(true, "Logout successful");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id),
                new Claim(ClaimTypes.Name, user.fullname),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
