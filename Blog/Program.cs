
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Blog.Iservice;
using Application.Blog.Services;
using BlogApp.Data;
using BlogApp.Model;
using BlogApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Description = "Please follow this format. Bearer space token in double literal",
                    Type = SecuritySchemeType.ApiKey
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //database configuration
            string connectionstring = builder.Configuration.GetConnectionString("connections");
            builder.Services.AddDbContext<BlogAppDBcontext>(options => options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)));

            //Configure setvices
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<IAuth, AuthServices>();
       
        
            //Identity configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<BlogAppDBcontext>()
                .AddSignInManager()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            //JWT settings
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                };
            });

            //Policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorPolicy", policy =>
                    policy.RequireRole("Author"));
                options.AddPolicy("GuestPolicy", policy =>
                        policy.RequireRole("Guest"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
       
            app.MapControllers();

            // Seed Roles and AUthor User
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Author", "Guest" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string defaultEmail = "author@blog.com";
                var authorUser = await userManager.FindByEmailAsync(defaultEmail);
                if (authorUser == null)
                {
                    authorUser = new ApplicationUser
                    {
                        UserName = "Author",
                        Email = defaultEmail,
                        EmailConfirmed = false,
                        fullname = "Default Author"
                    };
                    var result = await userManager.CreateAsync(authorUser, "Author@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(authorUser, "Author");
                    }
                }
            }

            app.Run();
        }
    }
}
