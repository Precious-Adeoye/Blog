using BlogApp.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class BlogAppDBcontext : IdentityDbContext<ApplicationUser>
    {
        public BlogAppDBcontext(DbContextOptions<BlogAppDBcontext>  options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
