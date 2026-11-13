using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải là Trưởng bộ môn hoặc Admin mới vào được cổng này
    [Authorize(Roles = "TruongBoMon,Admin")]
    public class DepartmentHeadController : Controller
    {
        private void SetCommonViewBag()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "PGS.TS. Trần Thị Mai";
            ViewBag.Username = User.Identity?.Name ?? "truongbomon";
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "TruongBoMon";
            ViewBag.Department = "Bộ môn Hệ thống Thông tin & Cơ sở Dữ liệu";
            ViewBag.Faculty = "Khoa Công nghệ Thông tin";
        }

        // 1. Dashboard Bàn làm việc Trưởng bộ môn
        public IActionResult Index()
        {
            SetCommonViewBag();
            return View();
        }

        // 2. Hàng đợi kiểm duyệt câu hỏi Cấp 1
        public IActionResult QuestionReviews(string? subject, string? teacher, string? status, int? page)
        {
            SetCommonViewBag();
            ViewBag.CurrentSubject = subject ?? "all";
            ViewBag.CurrentStatus = status ?? "pending";
            return View();
        }

        // 3. Hàng đợi kiểm duyệt đề thi Cấp 1
        public IActionResult ExamReviews(string? subject, string? status, int? page)
        {
            SetCommonViewBag();
            ViewBag.CurrentSubject = subject ?? "all";
            ViewBag.CurrentStatus = status ?? "pending";
            return View();
        }

        // 4. Ngân hàng câu hỏi bộ môn
        public IActionResult DepartmentQuestions(string? subject, string? bloom, string? search)
        {
            SetCommonViewBag();
            return View();
        }

        // 5. Tiến độ biên soạn câu hỏi của Giảng viên trong bộ môn
        public IActionResult LecturerProgress()
        {
            SetCommonViewBag();
            return View();
        }

        // 6. Lịch sử các lần duyệt câu hỏi và đề thi
        public IActionResult ReviewHistory()
        {
            SetCommonViewBag();
            return View();
        }

        // 7. Hồ sơ cá nhân Trưởng bộ môn
        public IActionResult Profile()
        {
            SetCommonViewBag();
            return View();
        }
    }
}

