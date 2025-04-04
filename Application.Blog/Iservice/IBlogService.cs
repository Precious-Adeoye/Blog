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
        Task<List<BlogResponseDTO>> GetAllBlogs();
        Task<BlogResponseDTO> GetBlogById(int blogId);
        Task<bool> CreateBlog(BlogDTO blogDto, string authorId);
        Task<bool> UpdateBlog(int blogId, BlogDTO blogDto);
        Task<bool> DeleteBlog(int blogId);
    }
}
