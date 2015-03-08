using System.ComponentModel.DataAnnotations;

namespace Tam.Blog.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        [RegularExpression("^[a-zA-Z0-9_-]{5, 15}$", ErrorMessage = "Tên đăng nhập gồm chữ cái, số, dấu -, _ và dài từ 5 đến 15 kí tự.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Mật khẩu từ 8 đến 20 kí tự.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng.")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(64, ErrorMessage = "Độ dài tối đa của email là 64 kí tự.")]
        [MaxLength(64, ErrorMessage = "Độ dài tối đa của email là 64 kí tự.")]
        public string Email { get; set; }


    }
}
