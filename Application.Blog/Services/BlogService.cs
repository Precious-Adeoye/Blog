using Application.Blog.DTOs;
using Application.Blog.Iservice;
using AutoMapper;
using BlogApp.Data;
using BlogApp.DTOs;
using BlogApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogAppDBcontext blogAppDBcontext;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogService(BlogAppDBcontext blogAppDBcontext, UserManager<ApplicationUser> userManager)
        {
            this.blogAppDBcontext = blogAppDBcontext;
            this.userManager = userManager;
        }

        public async Task<bool> AddComment(CommentDTO commentDto, string username)
        {
            var blog = await blogAppDBcontext.Blogs.FindAsync(commentDto.BlogId);
            if (blog == null) return false;

            var comment = new MainComment()
            {
                BlogId = commentDto.BlogId,
                UserName = username,
                Content = commentDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            blogAppDBcontext.MainComments.Add(comment);

            return await blogAppDBcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateBlog(BlogDTO blogDto, string authorId)
        {
            var blog = new Blog()
            {
                Title = blogDto.Title,
                Content = blogDto.Content,
                Tags = blogDto.Tags,
                AuthorID = authorId,
                CreatedAt = DateTime.UtcNow
            };
            await blogAppDBcontext.Blogs.AddAsync(blog);
            return await blogAppDBcontext.SaveChangesAsync() > 0;
        }

        public async Task<List<BlogResponseDTO>> GetAllBlogs(int page = 1, int pageSize = 10)
        {
            return await blogAppDBcontext.Blogs.Include(b => b.Author)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
             .Select(b => new BlogResponseDTO()
             {
                 Id = b.Id,
                 Title = b.Title,
                 Content = b.Content,
                 Tags = b.Tags,
                 AuthorName = b.Author.fullname
             }).ToListAsync();
        }

        public async Task<bool> ToggleLike(int blogId, string userId)
        {
            var checkLike = await blogAppDBcontext.Likes
                .FirstOrDefaultAsync(L => L.BlogId == blogId && L.UserId == userId);
            if (checkLike != null)
            {
                blogAppDBcontext.Likes.Remove(checkLike);
            }
            else
            {
                var like = new Like()
                {
                    BlogId = blogId,
                    UserId = userId
                };
                blogAppDBcontext.Likes.Add(like);
            }

            return await blogAppDBcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBlog(int id, string userId)
        {
            var blog = await blogAppDBcontext.Blogs.FindAsync(id);
            if (blog == null || blog.AuthorID != userId) return false;
            blogAppDBcontext.Blogs.Remove(blog);
            return await blogAppDBcontext.SaveChangesAsync() > 0;
        }
    }
}
