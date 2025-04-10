using Application.Blog.DTOs;
using Application.Blog.Iservice;
using AutoMapper;
using BlogApp.Data;
using BlogApp.DTOs;
using BlogApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogAppDBcontext blogAppDBcontext;

        public BlogService(BlogAppDBcontext blogAppDBcontext)
        {
            this.blogAppDBcontext = blogAppDBcontext;
        }

        public async Task<bool> AddComment(CommentDTO commentDto, string username)
        {
            var blog = await blogAppDBcontext.Blogs.FindAsync(commentDto.BlogId);
            if (blog == null)  return false;
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
          var blog  = new Blog()
          {
              Title = blogDto.Title,
              content = blogDto.Content,
              Tags = blogDto.Tags,
              AuthorID = authorId,
              CreatedAt = DateTime.UtcNow
          };
            await blogAppDBcontext.Blogs.AddAsync(blog);
            return await blogAppDBcontext.SaveChangesAsync() > 0;
        }

        public async Task<List<BlogResponseDTO>> GetAllBlogs()
        {
            return await blogAppDBcontext.Blogs
                  .Select(b => new BlogResponseDTO()
                  {
                      Id = b.Id,
                      Title = b.Title,
                      Content = b.content,
                      Tags = b.Tags,
                      AuthorName = b.Author.UserName
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
    }
}
