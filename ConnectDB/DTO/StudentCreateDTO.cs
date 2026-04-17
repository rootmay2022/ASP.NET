using System.ComponentModel.DataAnnotations;

public class StudentCreateDTO
{
    [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
    public string StudentCode { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string FullName { get; set; }

    public DateTime Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string? Email { get; set; }

    public string? Address { get; set; }
    public string? Status { get; set; }

    [Required]
    public int ClassId { get; set; }

    public int? UserId { get; set; } // Có thể null nếu chưa cấp tài khoản
}