using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải là Giảng viên hoặc Trưởng bộ môn hoặc Admin mới vào được cổng Giảng viên
    [Authorize(Roles = "GiangVien,TruongBoMon,Admin")]
    public class TeacherController : Controller
    {
        // 1. Trang Tổng quan Dashboard của Giảng viên
        public IActionResult Index()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 2. Danh sách câu hỏi
        public IActionResult Questions(string? filter, string? tab, string? search, int? page)
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 3. Soạn câu hỏi mới
        public IActionResult CreateQuestion()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 4. Chỉnh sửa câu hỏi
        public IActionResult EditQuestion(int? id)
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 5. Nhập câu hỏi từ file Word / Excel
        public IActionResult Import()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 6. Danh sách đề thi
        public IActionResult Exams()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 7. Tạo đề thi mới
        public IActionResult CreateExam()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 8. Hồ sơ cá nhân giảng viên
        public IActionResult Profile()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";

            return View();
        }

        // 9. Thông báo công việc (chuyển về Index kèm tab)
        public IActionResult Notifications()
        {
            return RedirectToAction("Index");
        }
    }
}

