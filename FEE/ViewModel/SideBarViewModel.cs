using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class SideBarViewModel
    {
        public string FunctionId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? SortOrder { get; set; }
        public string ParentId { get; set; }
        public string Icons { get; set; }

        public List<SideBarViewModel> NavItem { get; set; } = new List<SideBarViewModel>();
    }
}