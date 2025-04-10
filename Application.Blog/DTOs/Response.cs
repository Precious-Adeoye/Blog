using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blog.DTOs
{
    public class Response
    {
        public record class GeneralResponse(bool status, string message);
        public record class LogInResponse(bool flag, string message);
    }
}
