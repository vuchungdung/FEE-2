using FEE.Dtos;
using FEE.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEE.Models
{
    [Table("Posts")]
    public class Post : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string Alias { get; set; }
        public int CategoryId { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool HomeFlag { get; set; }
        public bool HotFlag { get; set; }
        public string Img { get; set; }
        public PostStatus Status { get; set; }
        public int DepartmentId { get; set; }
        public bool IsShow { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public string Tag { get; set; }
        public string Author { get; set; }

    }
}