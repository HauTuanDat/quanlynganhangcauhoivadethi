using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Bắt buộc phải là Giảng viên hoặc Trưởng bộ môn hoặc Admin mới vào được cổng Giảng viên
    [Authorize(Roles = "GiangVien,TruongBoMon,Admin")]
    public class TeacherController : Controller
    {
        // Gom các thiết lập ViewBag chung vào một phương thức để tái sử dụng
        private void SetCommonViewBag()
        {
            ViewBag.FullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "ThS. Nguyễn Văn A";
            ViewBag.Username = User.Identity?.Name ?? "giangvien";
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "GiangVien";
            ViewBag.CurrentSubject = "Cơ sở dữ liệu nâng cao (IT3020)";
        }

        // 1. Trang Tổng quan Dashboard của Giảng viên
        public IActionResult Index()
        {
            SetCommonViewBag();

            var model = new TeacherDashboardViewModel
            {
                TeacherFullName = ViewBag.FullName,
                TeacherTitle = "Giảng viên bộ môn",
                Username = ViewBag.Username,
                CurrentSubject = ViewBag.CurrentSubject ?? "Cơ sở dữ liệu nâng cao (IT3020)",
                Semester = "Học kỳ 1 (2026-2027)",
                RevisionRequiredCount = 15,
                PendingReviewCount = 42,
                ApprovedCount = 80,
                ApprovedPercentage = 80,
                CreatedExamsCount = 4,
                TargetQuestionCount = 100,
                BloomRecallCount = 40,
                BloomUnderstandCount = 30,
                BloomApplyCount = 20,
                BloomAnalyzeCount = 10,
                RevisionQuestions = new List<QuestionRevisionItem>
                {
                    new QuestionRevisionItem
                    {
                        Id = 42,
                        Code = "#CS-Q042",
                        Title = "Tính đa hình và từ khóa virtual trong C#",
                        Chapter = "Chương 3: Kế thừa & Đa hình",
                        Difficulty = "Thông hiểu",
                        ReviewerComment = "Đáp án B và D bị trùng ý diễn đạt. Cần viết lại câu hỏi rõ ràng hơn."
                    },
                    new QuestionRevisionItem
                    {
                        Id = 58,
                        Code = "#CS-Q058",
                        Title = "Cú pháp truy vấn LINQ to Objects",
                        Chapter = "Chương 4: LINQ & Generic",
                        Difficulty = "Vận dụng",
                        ReviewerComment = "Cần bổ sung chuẩn đầu ra CLO2 và giải thích đoạn mã ngắn."
                    },
                    new QuestionRevisionItem
                    {
                        Id = 71,
                        Code = "#CS-Q071",
                        Title = "Câu Đúng/Sai về bộ gom rác Garbage Collector",
                        Chapter = "Chương 2: Quản lý bộ nhớ",
                        Difficulty = "Nhận biết",
                        ReviewerComment = "Mệnh đề chưa chặt chẽ, sinh viên dễ hiểu nhầm về Dispose và Finalize."
                    }
                },
                TodoItems = new List<TeacherTodoItem>
                {
                    new TeacherTodoItem { Id = 1, Title = "Hoàn thiện 15 câu hỏi phản hồi trước ngày 20/11", Deadline = "20/11/2026", IsUrgent = true },
                    new TeacherTodoItem { Id = 2, Title = "Nộp ma trận đề thi cuối kỳ môn CSDL nâng cao", Deadline = "25/11/2026", IsUrgent = false }
                }
            };

            return View(model);
        }

        // 2. Danh sách câu hỏi
        public IActionResult Questions(string? filter, string? tab, string? search, int? page)
        {
            SetCommonViewBag();
            return View();
        }

        // 3. Soạn câu hỏi mới
        public IActionResult CreateQuestion()
        {
            SetCommonViewBag();
            return View();
        }

        // 4. Chỉnh sửa câu hỏi
        public IActionResult EditQuestion(int? id)
        {
            SetCommonViewBag();
            return View();
        }

        // 5. Nhập câu hỏi từ file Word / Excel
        public IActionResult Import()
        {
            SetCommonViewBag();
            return View();
        }

        // 6. Danh sách đề thi
        public IActionResult Exams()
        {
            SetCommonViewBag();
            return View();
        }

        // 7. Tạo đề thi mới
        public IActionResult CreateExam()
        {
            SetCommonViewBag();
            return View();
        }

        // 8. Hồ sơ cá nhân giảng viên
        public IActionResult Profile()
        {
            SetCommonViewBag();
            return View();
        }

        // 9. Thông báo công việc (chuyển về Index kèm tab)
        public IActionResult Notifications()
        {
            return RedirectToAction("Index");
        }
    }
}
