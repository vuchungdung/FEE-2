﻿using FEE.Models;
using FEE.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Controllers
{
    public class PostController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        public ActionResult PostDetail(int id)
        {
            var result = _db.Posts.Where(x => x.PostId == id).Select(x => new PostViewModel()
            {
                Name = x.Name,
                PostId = x.PostId,
                Img = x.Img,
                Description = x.Description,
                Content = x.Content,
                CreateDate = x.CreateDate,
                CategoryId = x.CategoryId.ToString(),
                Author = x.Author,
                Alias = x.Alias

            }).SingleOrDefault();
            TempData["categoryId"] = result.CategoryId;
            TempData["postId"] = id;
            var model = _db.Posts.Find(id);
            model.ViewCount += 1;
            _db.SaveChanges();
            return View(result);
        }
        public ActionResult Index(int? id,string tukhoa = "", int page = 1, int pageSize = 6, int? categoryId = 0, int tag = 0)
        {
            IPagedList<PostViewModel> result = null;
            
            var listItem = _db.Posts.Where(x=>x.Deleted == false && x.IsShow == true).Select(x => new PostViewModel()
            {
                Name = x.Name,
                PostId = x.PostId,
                MenuId = x.MenuId,
                Img = x.Img,
                Description = x.Description,
                CreateDate = x.CreateDate,
                TagId = x.Tag,
                CategoryId = x.CategoryId.ToString(),
                Alias = x.Alias

            }).OrderBy(x => x.CreateDate).ToList();

            if(id != 0)
            {
                listItem = listItem.Where(x => x.MenuId == id).ToList();
            }

            if (!String.IsNullOrEmpty(tukhoa))
            {
                listItem = listItem.Where(x => x.Name.ToUpper().Contains(tukhoa.ToUpper())).ToList();
            }

            if(categoryId != 0)
            {
                ViewBag.categoryId = categoryId;
                listItem = listItem.Where(x => x.CategoryId == categoryId.ToString()).ToList();
            }

            if(tag != 0)
            {
                ViewBag.tag = tag;
                listItem = listItem.Where(x => x.TagId.Contains(tag.ToString())).ToList();
            }

            ViewBag.Count = listItem.Count();
            result = listItem.ToPagedList(page, pageSize);            
            return View(result);
        }

        public ActionResult Search(string tukhoa = "", int page = 1, int pageSize = 6)
        {
            IPagedList<PostViewModel> result = null;

            var listItem = _db.Posts.Where(x => x.Deleted == false && x.IsShow == true).Select(x => new PostViewModel()
            {
                Name = x.Name,
                PostId = x.PostId,
                MenuId = x.MenuId,
                Img = x.Img,
                Description = x.Description,
                CreateDate = x.CreateDate,
                TagId = x.Tag,
                CategoryId = x.CategoryId.ToString(),
                Alias = x.Alias

            }).OrderBy(x => x.CreateDate).ToList();          

            if (!String.IsNullOrEmpty(tukhoa))
            {
                listItem = listItem.Where(x => x.Name.ToUpper().Contains(tukhoa.ToUpper())).ToList();
            }           

            ViewBag.Count = listItem.Count();
            result = listItem.ToPagedList(page, pageSize);
            return View(result);
        }


        public ActionResult Category()
        {
            var listItem = _db.Categories.Select(x => new CategoryViewModel()
            {

                Name = x.Name,
                CategoryId = x.CategoryId,
            }).ToList().OrderBy(x => x.CategoryId).ToList();

            return PartialView(listItem);
        }
        public ActionResult PostRelate()
        {
            var listItem = _db.Posts.Where(x => x.Deleted == false && x.IsShow == true).Select(x => new PostViewModel()
            {
                Name = x.Name,
                Alias=x.Alias,
                PostId = x.PostId,
                Img = x.Img,
                Description = x.Description,
                CreateDate = x.CreateDate,
                CategoryId = x.CategoryId.ToString()

            }).OrderBy(x => x.CreateDate).ToList();
            listItem = listItem.Where(x => x.CategoryId == TempData["categoryId"].ToString() && x.PostId != Convert.ToInt32(TempData["postId"])).ToList();
            return PartialView(listItem);
        }
        public ActionResult Tag()
        {
            var keyword = TempData["postId"].ToString();
            var post = _db.Posts.Find(Convert.ToInt32(keyword));
            var listTags = post.Tag.Split(',').ToList();
            List<TagViewModel> listItem = new List<TagViewModel>();
            foreach(var item in listTags)
            {
                listItem.Add(_db.Tags.Where(x => x.Id.ToString() == item).Select(x => new TagViewModel() { TagId = x.Id, Name = x.Name }).FirstOrDefault());
            }
            return PartialView(listItem);
        }

        public ActionResult AllTag()
        {
            var listItem = _db.Tags.Select(x => new TagViewModel()
            {
                Name = x.Name,
                TagId = x.Id,
            }).ToList();
            return PartialView(listItem);
        }
    }
}