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
        [Display(Name = "Bản nháp")]
        Doing = 1,
        [Display(Name = "Hoàn thành")]
        Actived = 0,
    }
}