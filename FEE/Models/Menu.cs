using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEE.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        public string Name { get; set; }
        public string URL { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public string Link { get; set; }
        public int ParentId { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? UpdateDate { get; set; }
    }
}