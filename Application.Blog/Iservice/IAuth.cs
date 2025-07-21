using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Blog.DTOs;
using static Application.Blog.DTOs.Response;

namespace Application.Blog.Iservice
{
    public interface IAuth
    {
        Task<GeneralResponse> RegisterAsync(RegisterDto Dto);
        Task<LogInResponse> LoginAsync(LoginDto Dto);
        Task<GeneralResponse> LogoutAsync();
    }
}
