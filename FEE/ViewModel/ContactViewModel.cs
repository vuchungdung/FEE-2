using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập phản hồi")]
        public string Message { get; set; }

    }
}