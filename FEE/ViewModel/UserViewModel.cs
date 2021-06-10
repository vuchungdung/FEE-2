using FEE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }

    }
}