using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEE.Enums
{
    public enum UserStatus
    {
        [Display(Name ="Hoạt động")]
        Activated,
        [Display(Name = "Đang khóa")]
        Blocked,
    }
}