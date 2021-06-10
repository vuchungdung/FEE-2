using FEE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.ViewModel
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        [Required(ErrorMessage = "Chưa nhập tiêu đề bài viết")]
        public string Name { get; set; }
        public string Alias { get; set; }
        [Required(ErrorMessage = "Chưa chọn loại bài viết")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "Chưa chọn danh mục")]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Chưa nhập mô tả bài viết")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Chưa nhập nội dung bài viết")]
        public string Content { get; set; }

        public bool Delete { get; set; }

        public string Author { get; set; }
        public bool HomeFlag { get; set; }
        public bool HotFlag { get; set; }
        public bool IsShow { get; set; }
        [Required(ErrorMessage = "Chưa có hình ảnh bài viết")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Chưa chọn trạng thái bài viết")]
        public PostStatus Status { get; set; }
        public string MoreImgs { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string TagId { get; set; }
        [Required(ErrorMessage = "Chưa có tag cho bài viết")]
        public List<string> TagIds { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public string Date { get; set; }
        public List<SelectListItem> ListCategories { get; set; }
        public SelectList ListMenus { get; set; }
    }
}