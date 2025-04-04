using Application.Blog.DTOs;
using Application.Blog.Iservice;
using AutoMapper;
using BlogApp.Data;
using BlogApp.DTOs;

namespace BlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogAppDBcontext blogAppDBcontext;
        private readonly IMapper mapper;

        public BlogService(BlogAppDBcontext blogAppDBcontext, IMapper mapper)
        {
            this.blogAppDBcontext = blogAppDBcontext;
            this.mapper = mapper;
        }

        public Task<bool> CreateBlog(BlogDTO blogDto, string authorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBlog(int blogId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlogResponseDTO>> GetAllBlogs()
        {
            throw new NotImplementedException();
        }

        public Task<BlogResponseDTO> GetBlogById(int blogId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBlog(int blogId, BlogDTO blogDto)
        {
            throw new NotImplementedException();
        }
    }
}
