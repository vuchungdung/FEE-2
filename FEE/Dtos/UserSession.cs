using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.Dtos
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public string Image { get; set; }
    }
}