using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.ViewModel
{
    public class PermissionScreenViewModel
    {
        public string FunctionId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public bool HasCreate { get; set; }
        public bool HasUpdate { get; set; }
        public bool HasDelete { get; set; }
        public bool HasView { get; set; }
        public bool HasReply { get; set; }
        public bool HasTrash { get; set; }
        public List<PermissionScreenViewModel> Childrens { get; set; } = new List<PermissionScreenViewModel>();
    }
}