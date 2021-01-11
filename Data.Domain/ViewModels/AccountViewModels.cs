using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Data.Domain.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور فعلی")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} باید از 6 حرف یا عدد بیشتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "کلمه عبور جدید با تکرار کلمه عبور جدید مطابقت ندارد.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "نام کاربری :")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور :")]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "نام کاربری :")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "آدرس الکترونیکی :")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "توجه: {0} باید از {1} حرف یا عدد بیشتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تایید کلمه عبور :")]
        [Compare("Password", ErrorMessage = "کلمه عبور جدید با تکرار کلمه عبور جدید مطابقت ندارد.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "نام کاربری :")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "آدرس الکترونیکی :")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "توجه: {0} باید از {1} حرف یا عدد بیشتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تایید کلمه عبور :")]
        [Compare("Password", ErrorMessage = "کلمه عبور جدید با تکرار کلمه عبور جدید مطابقت ندارد.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}