using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 1. Cấu hình Entity Framework Core với SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Dịch vụ mã hóa mật khẩu an toàn của Microsoft (PBKDF2)
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // 3. Cấu hình Cookie Authentication (Quản lý phiên đăng nhập)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";               // Chưa đăng nhập mà đòi vào trang cấm -> đá về Login
                    options.AccessDeniedPath = "/Account/AccessDenied"; // Không đủ quyền truy cập -> đá về đây
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);      // Phiên đăng nhập tồn tại 8 tiếng
                    options.SlidingExpiration = true;                   // Tự động gia hạn nếu người dùng còn hoạt động
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // ⚠️ QUAN TRỌNG: UseAuthentication phải đứng TRƯỚC UseAuthorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
