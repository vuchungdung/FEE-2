using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ListMenuIds { get; set; }
        public List<MenuViewModel> Menus { get; set; }
    }
}