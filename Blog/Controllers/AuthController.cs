using Application.Blog.DTOs;
using Application.Blog.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid registration data.");
            }
            var response = await _authService.RegisterAsync(dto);
            if (!response.status)
            {
                return BadRequest(response.message);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid login data.");
            }
            var response = await _authService.LoginAsync(dto);
            if (!response.flag)
            {
                return Unauthorized(response.message);
            }
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var response = await _authService.LogoutAsync();
            if (!response.status)
            {
                return BadRequest(response.message);
            }
            return Ok(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("AuthController is working!");
        }
    }
}
