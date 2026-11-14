using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("ActivityLogs")]
    public class ActivityLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string UserRole { get; set; } = string.Empty;
        // Mô tả loại hành động (ví dụ: "Login", "Logout", "Create", "Update", "Delete")
        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; } = string.Empty;
        // Mục tiêu của hành động (ví dụ: "User", "Question", "Exam")
        [MaxLength(50)]
        public string TargetType { get; set; } = string.Empty;
        public int? TargetId { get; set; }
        [Required]
        [MaxLength(150)]
        // mô tả ngắn gọn về hành động (ví dụ: "Đăng nhập thành công", "Cập nhật thông tin cá nhân")
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        // mô tả chi tiết về hành động (ví dụ: "Người dùng đã đăng nhập thành công", "Cập nhật thông tin cá nhân")
        public string Description { get; set; } = string.Empty;
        // nơi làm việc và khi nào 
        [MaxLength(45)]
        public string ? IpAddress { get; set; }
        public DateTime CreateAt  { get; set; } = DateTime.Now;
    }
}
