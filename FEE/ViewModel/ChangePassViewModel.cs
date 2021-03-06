using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class ChangePassViewModel
    {
        [Required(ErrorMessage = "Chưa nhập mật khẩu")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Bạn nhập mật khẩu mới không trùng nhau!")]
        public string repassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu cũ")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string oldpassword { get; set; }
    }
}