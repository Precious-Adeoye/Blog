using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blog.DTOs
{
    public record class UserSession(string? id, string? fullname, string? email,string? role);
   
}
