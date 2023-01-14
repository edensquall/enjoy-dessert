using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Display(Name = "使用者名稱")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? UserName { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        [RegularExpression(@"(?=^.{6,}$)(?=.*[a-z])(?!.*\s).*$",
            ErrorMessage = "{0} 必須最少6個字符，包含1個小寫字母")]
        public string? Password { get; set; }
        [Display(Name = "顯示名稱")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? DisplayName { get; set; }
        [Display(Name = "電話")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Phone(ErrorMessage = "{0} 格式錯誤")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} 為必填")]
        [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
        public string? Email { get; set; }
    }
}