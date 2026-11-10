using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải đăng nhập mới được vào trang chủ này
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Lấy thông tin họ tên và vai trò từ Cookie
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name;
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";

            return View();
        }
    }
}

