using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class RePasswordViewModel
    {
        [Required(ErrorMessage ="Nhập đầy đủ thông tin")]
        public string Username { get; set; }
    }
}