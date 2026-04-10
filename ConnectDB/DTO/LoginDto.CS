public class LoginDto
{
    // Phải có "= string.Empty" để tránh lỗi nhận diện dữ liệu
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}