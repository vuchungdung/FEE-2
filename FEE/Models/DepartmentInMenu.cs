using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEE.Models
{
    [Table("DepartmentInMenus")]
    public class DepartmentInMenu
    {
        [Key]
        [Column(Order = 1)]
        public int DepartmentId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int MenuId { get; set; }
    }
}