using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Blog.DTOs;
using BlogApp.DTOs;

namespace Application.Blog.Iservice
{
    public interface IBlogService
    {
        Task<bool> CreateBlog(BlogDTO blogDto, string authorId);
        Task<List<BlogResponseDTO>> GetAllBlogs(int page = 1, int pageSize = 10);
        Task<bool> AddComment(CommentDTO commentDto, string username);
        Task<bool> ToggleLike(int blogId, string userId);
        Task<bool> DeleteBlog(int id, string userId);
    }
}
