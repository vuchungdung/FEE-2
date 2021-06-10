using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class HomeViewModel
    {
        public List<PostViewModel> ThongBaos { get; set; }
        public List<PostViewModel> TinTucs { get; set; }
        public List<PostViewModel> TinNoiBats { get; set; }
        public List<PostViewModel> TinTuyenDung { get; set; }
        public PostViewModel BaiGhim { get; set; }
        public List<SlideViewModel> Slides { get; set; }
    }
}