using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class SlideViewModel
    {
        public int SlideId { get; set; }
        public string Img { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateBy { get; set; }
        public int CreateBy { get; set; }
    }
}