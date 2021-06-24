using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEE.Models
{
    [Table("WebInfo")]
    public class WebInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WebInfoId { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Zalo { get; set; }
        public string Copyright { get; set; }
        public string Website { get; set; }
        public string Time { get; set; }
        public string Note { get; set; }
    }
}