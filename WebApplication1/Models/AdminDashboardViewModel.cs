using System.Collections.Generic;

namespace WebApplication1.Models
{
    /// <summary>
    /// ViewModel chứa dữ liệu tổng quan cho trang Dashboard Quản trị (Index.cshtml)
    /// </summary>
    public class AdminDashboardViewModel
    {
        // 4 Thống kê chính trên Banner Hero
        public int TotalUsers { get; set; }               // Thẻ 1: Tổng số người dùng
        public int ActiveUsers { get; set; }              // Thẻ 2: Số người đang hoạt động
        public int TotalSubjects { get; set; }            // Thẻ 3: Tổng số môn học
        public int TotalDepartments { get; set; }         // Thẻ 4: Tổng số khoa / bộ môn

        // Tỷ lệ phần trăm tăng trưởng hiển thị trên các huy hiệu (Badge %)
        public int UserGrowthPercent { get; set; } = 8;
        public int ActiveGrowthPercent { get; set; } = 12;
        public int SubjectGrowthPercent { get; set; } = 5;
        public int DepartmentGrowthPercent { get; set; } = 3;

        // Thông tin niên khóa & học kỳ
        public string AcademicYear { get; set; } = "Năm học 2026 - 2027";
        public string Semester { get; set; } = "Học kỳ 1";

        // Thông tin định danh của Quản trị viên
        public string AdminFullName { get; set; } = "Quản trị viên";
        public string AdminRole { get; set; } = "Admin";

        // Biểu đồ đường: Khối lượng công việc theo trạng thái (7 ngày gần nhất)
        public List<RecentActivityItem> RecentActivities { get; set; } = new();
        public List<QuickNoticeItem> QuickNotices { get; set; } = new();
        public WorkloadChartData WorkloadChart { get; set; } = new();
    }

    /// <summary>
    /// Dữ liệu biểu đồ đường phản ánh nhịp độ làm việc của giảng viên trong tuần
    /// </summary>
    public class WorkloadChartData
    {
        // Danh sách 7 ngày (trục hoành X, ví dụ: "08/11", "09/11"...)
        public List<string> Labels { get; set; } = new();

        // Đường 1: Số câu hỏi / công việc mới nộp chờ phê duyệt
        public List<int> PendingList { get; set; } = new();

        // Đường 2: Số câu hỏi đang được phản hồi / giảng viên đang sửa đổi
        public List<int> InProgressList { get; set; } = new();

        // Đường 3: Số câu hỏi đã được thẩm định & phê duyệt vào ngân hàng
        public List<int> CompletedList { get; set; } = new();
    }
     // Danh sách 5 hoạt động gần đây của giảng viên/hệ thống
      
    public class RecentActivityItem // nghĩa là "Mục hoạt động gần đây"
    {
        public string Title { get; set; } = string.Empty; // Tiêu đề hoạt động (ví dụ: "Người dùng mới đăng ký", "Câu hỏi được phê duyệt")
        public string Description { get; set; } =string.Empty;
        public string Timestamp { get; set; } = string.Empty; // Ví dụ: "2 giờ trước", "Hôm qua", "3 ngày trước"
        public string IconClass { get; set; } = "fa-file-lines"; // Icon FontAwesome (vd: "fa-file-lines", "fa-user-plus")
        public string ColorClass { get; set; } = "primary";     // Màu sắc (primary, success, info, warning, danger)
    }
    public  class QuickNoticeItem // nghĩa là "Mục thông báo nhanh"

    {
        public string Message { get; set; } = string.Empty; // Nội dung thông báo
        public string Type { get; set; } = "info"; // Loại thông báo (info, success, warning, danger)
public string TimeAgo { get; set; } = string.Empty; // Thời gian thông báo (ví dụ: "1 giờ trước", "Hôm qua")}
    }
}
