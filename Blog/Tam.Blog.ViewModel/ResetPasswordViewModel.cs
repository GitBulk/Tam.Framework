﻿using System.ComponentModel.DataAnnotations;

namespace Tam.Blog.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(15, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [MaxLength(15, ErrorMessage = "Mật khẩu có độ dài tối đa 15 kí tự.")]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng.")]
        public string ConfirmPassword { get; set; }
    }
}