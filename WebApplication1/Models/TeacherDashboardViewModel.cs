using System.Collections.Generic;

namespace WebApplication1.Models
{
    /// <summary>
    /// ViewModel đại diện cho dữ liệu hiển thị trên Dashboard Giảng viên (Teacher/Index.cshtml)
    /// </summary>
    public class TeacherDashboardViewModel
    {
        // Thông tin giảng viên & bộ môn
        public string TeacherFullName { get; set; } = "ThS. Nguyễn Văn A";
        public string TeacherTitle { get; set; } = "Giảng viên bộ môn";
        public string Username { get; set; } = "giangvien";
        public string CurrentSubject { get; set; } = "Cơ sở dữ liệu nâng cao (IT3020)";
        public string Semester { get; set; } = "Học kỳ 1 (2026-2027)";

        // 4 Thống kê thẻ chỉ số cốt lõi (Stat Cards)
        public int RevisionRequiredCount { get; set; } = 15;     // 1. Cần chỉnh sửa (Feedback từ TBM)
        public int PendingReviewCount { get; set; } = 42;        // 2. Đang chờ TBM duyệt
        public int ApprovedCount { get; set; } = 80;             // 3. Đã duyệt vào ngân hàng
        public int ApprovedPercentage { get; set; } = 80;        // Tỷ lệ % đạt chuẩn
        public int CreatedExamsCount { get; set; } = 4;          // 4. Đề thi đã tạo

        // Phân bổ ma trận độ khó Bloom
        public int TargetQuestionCount { get; set; } = 100;
        public int BloomRecallCount { get; set; } = 40;          // Nhận biết
        public int BloomUnderstandCount { get; set; } = 30;      // Thông hiểu
        public int BloomApplyCount { get; set; } = 20;           // Vận dụng
        public int BloomAnalyzeCount { get; set; } = 10;         // Vận dụng cao

        // Danh sách câu hỏi cần sửa đổi (Tab 1)
        public List<QuestionRevisionItem> RevisionQuestions { get; set; } = new();

        // Danh sách công việc cần làm (Tab 4)
        public int TodoCount => TodoItems?.Count ?? 5;
        public List<TeacherTodoItem> TodoItems { get; set; } = new();
    }

    /// <summary>
    /// Thông tin từng câu hỏi bị Trưởng bộ môn trả về yêu cầu sửa lại
    /// </summary>
    public class QuestionRevisionItem
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Chapter { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string ReviewerComment { get; set; } = string.Empty;
    }

    /// <summary>
    /// Việc cần làm của giảng viên
    /// </summary>
    public class TeacherTodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Deadline { get; set; } = string.Empty;
        public bool IsUrgent { get; set; }
    }
}

