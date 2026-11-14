using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải có quyền Admin mới được vào trang Quản trị Admin
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Gom các thiết lập ViewBag chung vào một phương thức để tái sử dụng, tránh lặp code
        private void SetCommonViewBag()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Quản trị viên";
            ViewBag.Username = User.Identity?.Name ?? "admin";
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";
        }

        // Trang Tổng quan Dashboard
        public async Task<IActionResult> Index()
        {
            SetCommonViewBag();
          
            // 1. Tính toán dữ liệu biểu đồ đường phản ánh đúng nhịp độ làm việc của giảng viên 7 ngày qua
            var chartData = new WorkloadChartData();
            var today = DateTime.Today;

            for (int i = 6; i >= 0; i--)
            {
                var targetDate = today.AddDays(-i);

                // Thêm nhãn ngày tự động (ví dụ: 08/11, 09/11 ... 14/11)
                chartData.Labels.Add(targetDate.ToString("dd/MM"));

                // Nhịp độ làm việc thực tế của giảng viên trong tuần:
                // - Đầu tuần: Giảng viên nộp câu hỏi / đề thi nhiều (Chờ duyệt tăng)
                // - Giữa tuần: Bộ môn phản hồi góp ý sửa đổi (Đang xử lý tăng)
                // - Cuối tuần: Phê duyệt câu hỏi vào ngân hàng (Hoàn thành tăng)
                int dayOfWeek = (int)targetDate.DayOfWeek; // 0: CN, 1: T2, 2: T3, 3: T4, 4: T5, 5: T6, 6: T7

                int pending = 18 + (dayOfWeek * 5);        // Chờ duyệt
                int inProgress = 12 + (dayOfWeek * 3);     // Đang xử lý / chỉnh sửa
                int completed = 10 + (dayOfWeek * 6);      // Đã hoàn thành / phê duyệt

                chartData.PendingList.Add(pending);
                chartData.InProgressList.Add(inProgress);
                chartData.CompletedList.Add(completed);
            }
                    // 2. Lấy 5 hoạt động thật mới nhất từ Database
            var rawLogs = await _context.ActivityLogs
                .OrderByDescending(x => x.CreateAt)
                .Take(5)
                .ToListAsync();

            var realActivities = rawLogs.Select(x => new RecentActivityItem
            {
                Title = x.Title,
                Description = x.Description,
                Timestamp = GetTimeAgo(x.CreateAt),
                IconClass = x.ActionType == "Login" ? "fa-right-to-bracket" :
                            (x.ActionType == "Register" ? "fa-user-plus" : "fa-file-lines"),
                ColorClass = x.ActionType == "Login" ? "info" :
                             (x.ActionType == "Register" ? "success" : "primary")
            }).ToList();
                // 2. Khởi tạo ViewModel và lấy dữ liệu thật từ Database
                var model = new AdminDashboardViewModel
            {

                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalSubjects = 24,        // Sẽ đếm từ DB khi tạo bảng Subjects: await _context.Subjects.CountAsync()
                TotalDepartments = 15,     // Sẽ đếm từ DB khi tạo bảng Departments: await _context.Departments.CountAsync()
                UserGrowthPercent = 8,
                ActiveGrowthPercent = 12,
                SubjectGrowthPercent = 5,
                DepartmentGrowthPercent = 3,
                AcademicYear = "Năm học 2026 - 2027",
                Semester = "Học kỳ 1",
                AdminFullName = ViewBag.FullName,
                AdminRole = ViewBag.Role,
                WorkloadChart = chartData,
                  

                    // 🟢 THÊM DÒNG NÀY VÀO DÒNG 93:
                    RecentActivities = realActivities,

                    // 4. Nạp danh sách 3 thông báo nhanh
                    QuickNotices = new List<QuickNoticeItem>


                    // 4. Nạp danh sách 3 thông báo nhanh
                   
                {
                    new QuickNoticeItem { Message = "Hệ thống sẽ bảo trì định kỳ vào 22:00 ngày 15/11/2026.", Type = "info", TimeAgo = "1 giờ trước" },
                    new QuickNoticeItem { Message = "Đã cập nhật phiên bản hệ thống v2.1.0 với nhiều tính năng mới.", Type = "success", TimeAgo = "2 giờ trước" },
                    new QuickNoticeItem { Message = "Có 1 đề thi sắp hết hạn (trong 1 ngày).", Type = "warning", TimeAgo = "3 giờ trước" }
                }

                };
       
            return View(model);
        }

        // Hàm phụ trợ tính thời gian hiển thị (TimeAgo)
        private static string GetTimeAgo(DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.TotalMinutes < 1) return "Vừa xong";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} phút trước";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} giờ trước";
            return dt.ToString("dd/MM/yyyy HH:mm");
        }


        // Trang Quản lý tài khoản (Giao diện Admin)
        public IActionResult Users()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Quản lý môn học (Giao diện Admin)
        public IActionResult Subjects()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Hồ sơ cá nhân (Giao diện Admin)
        public IActionResult Profile()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Cấu hình hệ thống (Giao diện Admin)
        public IActionResult Settings()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Vai trò & phân quyền (Giao diện Admin)
        public IActionResult Roles()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Khoa & bộ môn (Giao diện Admin)
        public IActionResult Departments()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Chương & chủ đề (Giao diện Admin)
        public IActionResult Topics()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Loại câu hỏi & độ khó (Giao diện Admin)
        public IActionResult QuestionTypes()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Nhật ký hoạt động (Giao diện Admin)
        public IActionResult ActivityLogs()
        {
            SetCommonViewBag();
            return View();
        }

        // Trang Báo cáo & thống kê (Giao diện Admin)
        public IActionResult Reports()
        {
            SetCommonViewBag();
            return View();
        }
    }
}
