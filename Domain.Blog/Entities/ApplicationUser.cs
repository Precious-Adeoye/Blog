using Microsoft.AspNetCore.Identity;

namespace BlogApp.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string fullname { get; set; }
    }
}
