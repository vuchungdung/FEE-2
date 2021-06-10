using FEE.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEE.Models
{
    [Table("Categories")]
    public class Category : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}