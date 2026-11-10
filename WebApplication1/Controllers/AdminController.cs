using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải đăng nhập mới được vào trang Quản trị Admin
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Lấy thông tin họ tên và vai trò từ Cookie
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Quản lý tài khoản (Giao diện Admin)
        public IActionResult Users()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Quản lý môn học (Giao diện Admin)
        public IActionResult Subjects()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Hồ sơ cá nhân (Giao diện Admin)
        public IActionResult Profile()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Nguyễn Văn An";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Administrator";

            return View();
        }

        // Trang Cấu hình hệ thống (Giao diện Admin)
        public IActionResult Settings()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Vai trò & phân quyền (Giao diện Admin)
        public IActionResult Roles()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Khoa & bộ môn (Giao diện Admin)
        public IActionResult Departments()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Chương & chủ đề (Giao diện Admin)
        public IActionResult Topics()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Loại câu hỏi & độ khó (Giao diện Admin)
        public IActionResult QuestionTypes()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Nhật ký hoạt động (Giao diện Admin)
        public IActionResult ActivityLogs()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }

        // Trang Báo cáo & thống kê (Giao diện Admin)
        public IActionResult Reports()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";

            return View();
        }
    }
}

