using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải là Phòng Khảo Thí hoặc Admin mới vào được cổng này
    [Authorize(Roles = "PhongKhaoThi,Admin")]
    public class ExamOfficeController : Controller
    {
        private void SetCommonViewBag()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Hoàng Thị Lan";
            ViewBag.Username = User.Identity?.Name ?? "khaothi";
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "PhongKhaoThi";
            ViewBag.Department = "Phòng Khảo thí & Đảm bảo Chất lượng Giáo dục";
            ViewBag.Institution = "Trường Đại học Công nghệ";
        }

        // 1. Dashboard Khảo thí
        public IActionResult Index()
        {
            SetCommonViewBag();
            return View();
        }

        // 2. Hàng đợi kiểm duyệt câu hỏi Cấp 2 (Chuyển sang Approved)
        public IActionResult QuestionReviews(string? faculty, string? subject, string? status, int? page)
        {
            SetCommonViewBag();
            ViewBag.CurrentSubject = subject ?? "all";
            ViewBag.CurrentStatus = status ?? "pending_examoffice";
            return View();
        }

        // 3. Hàng đợi kiểm duyệt đề thi Cấp 2 (Cấp phép Phát hành)
        public IActionResult ExamReviews(string? faculty, string? subject, string? status, int? page)
        {
            SetCommonViewBag();
            ViewBag.CurrentSubject = subject ?? "all";
            ViewBag.CurrentStatus = status ?? "pending_examoffice";
            return View();
        }

        // 4. Phát hành đề thi & Xuất Word/PDF tách biệt đáp án
        public IActionResult Publishing()
        {
            SetCommonViewBag();
            return View();
        }

        // 5. Quản lý đợt in ấn & Niêm phong bàn giao
        public IActionResult PrintBatches()
        {
            SetCommonViewBag();
            return View();
        }

        // 6. Lịch sử bàn giao và lưu trữ
        public IActionResult HandoverHistory()
        {
            SetCommonViewBag();
            return View();
        }

        // 7. Báo cáo thống kê toàn trường
        public IActionResult Reports()
        {
            SetCommonViewBag();
            return View();
        }

        // 8. Hồ sơ cá nhân chuyên viên khảo thí
        public IActionResult Profile()
        {
            SetCommonViewBag();
            return View();
        }
    }
}

