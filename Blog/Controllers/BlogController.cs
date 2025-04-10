using Application.Blog.DTOs;
using Application.Blog.Iservice;
using BlogApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService BlogService;
        private readonly ILogger<BlogController> logger;

        public BlogController(IBlogService blogService, ILogger<BlogController> logger)
        {
            BlogService = blogService;
            this.logger = logger;
        }

        [HttpPost("Addcomment")]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDto)
        {
            var userName = User.Identity.Name;
            if (userName == null) return BadRequest("User not found");
            var result = await BlogService.AddComment(commentDto, userName);
            if (result) return Ok("Comment added successfully");
            return BadRequest("Failed to add comment");
        }

        [HttpPost("CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDTO blogDto)
        {
            var authorId = User.FindFirst("Id")?.Value;
            if (authorId == null) return BadRequest("User not found");
            var result = await BlogService.CreateBlog(blogDto, authorId);
            if (result) return Ok("Blog created successfully");
            return BadRequest("Failed to create blog");
        }

        [HttpGet("GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await BlogService.GetAllBlogs();
            if (blogs == null) return NotFound("No blogs found");
            return Ok(blogs);
        }

        [HttpPost("Like/{BlogId}")]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO likeDto)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null) return BadRequest("User not found");
            var result = await BlogService.ToggleLike(likeDto.BlogId, userId);
            if (result) return Ok("Like toggled successfully");
            return BadRequest("Failed to toggle like");
        }

    }
}
