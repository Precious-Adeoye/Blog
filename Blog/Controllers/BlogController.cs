using Application.Blog.DTOs;
using Application.Blog.Iservice;
using BlogApp.DTOs;
using BlogApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService BlogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            BlogService = blogService;
            _userManager = userManager;
        }

        [HttpPost("Addcomment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await BlogService.AddComment(commentDto, user.UserName);
            if (!result) return BadRequest("Failed to add comment");
            return Ok("Comment added successfully");
        }

        [HttpPost("CreateBlog")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDTO blogDto)
        {
            var authorId = await _userManager.GetUserAsync(User);
            if (authorId == null) return Unauthorized();

            if (string.IsNullOrEmpty(blogDto.Title) || string.IsNullOrEmpty(blogDto.Content))
            {
                return BadRequest("Title and Content are required");
            }

            var result = await BlogService.CreateBlog(blogDto, authorId.Id);
            if (!result) return BadRequest("Failed to create blog");
            return Ok("Blog created successfully");
        }

        [HttpGet("GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var blogs = await BlogService.GetAllBlogs();
            if (blogs == null) return NotFound("No blogs found");
            return Ok(blogs);
        }

        [HttpPost("Like/{BlogId}")]
        [Authorize]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO likeDto)
        {
            var userId = await _userManager.GetUserAsync(User);
            if (userId == null) return Unauthorized();

            var result = await BlogService.ToggleLike(likeDto.BlogId, userId.Id);
            if (!result) return BadRequest("Failed to toggle like");

            return Ok("Like toggled successfully");
        }

        [HttpDelete("DeleteBlog/{id}")]
        [Authorize(Roles = "Author")]

        public async Task<IActionResult> DeleteBlog(int id)
        {
            var userId = await _userManager.GetUserAsync(User);
            if (userId == null) return Unauthorized();
            var result = await BlogService.DeleteBlog(id, userId.Id);
            if (!result) return BadRequest("Failed to delete blog");
            return Ok("Blog deleted successfully");

        }
    }
}
