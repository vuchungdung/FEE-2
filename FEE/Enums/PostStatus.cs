using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FEE.Enums
{
    public enum PostStatus
    {
        [Display(Name = "Đang soạn")]
        Doing = 0,
        [Display(Name = "Đã duyệt")]
        Actived = 1,
        [Display(Name = "Chờ duyệt")]
        NoActive = 2,
    }
}