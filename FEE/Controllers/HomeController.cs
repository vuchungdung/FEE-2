using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Controllers
{
    public class HomeController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        public ActionResult Index()
        {
            var listResult = new HomeViewModel();

            listResult.ThongBaos = _db.Posts.Where(x => x.CategoryId == 3 && x.Deleted == false && x.IsShow == true)
                                            .Select(x => new PostViewModel()
                                            {
                                                PostId = x.PostId,
                                                Name = x.Name,
                                                Description = x.Description,
                                                Img = x.Img,
                                                CreateDate = x.CreateDate,
                                                Alias = x.Alias

                                            }).OrderByDescending(x => x.CreateDate).ToList();
            listResult.TinNoiBats = _db.Posts.Where(x => x.Deleted == false && x.IsShow == true && x.HomeFlag == true && x.HotFlag == false)
                                            .Select(x => new PostViewModel()
                                            {
                                                PostId = x.PostId,
                                                Name = x.Name,
                                                Description = x.Description,
                                                Img = x.Img,
                                                CreateDate = x.CreateDate,
                                                Alias = x.Alias

                                            }).OrderByDescending(x => x.CreateDate).ToList();
            listResult.TinTucs = _db.Posts.Where(x => x.Deleted == false && x.CategoryId == 1 && x.IsShow == true)
                                            .Select(x => new PostViewModel()
                                            {
                                                PostId = x.PostId,
                                                Name = x.Name,
                                                Description = x.Description,
                                                Img = x.Img,
                                                CreateDate = x.CreateDate,
                                                Alias = x.Alias

                                            }).OrderByDescending(x => x.CreateDate).ToList();
            listResult.TinTuyenDung = _db.Posts.Where(x => x.Deleted == false && x.CategoryId == 4 && x.IsShow == true)
                                            .Select(x => new PostViewModel()
                                            {
                                                PostId = x.PostId,
                                                Name = x.Name,
                                                Description = x.Description,
                                                Img = x.Img,
                                                CreateDate = x.CreateDate,
                                                Alias = x.Alias

                                            }).OrderByDescending(x => x.CreateDate).ToList();
            listResult.BaiGhim = _db.Posts.Where(x => x.HotFlag == true && x.Deleted == false && x.IsShow == true)
                                            .Select(x => new PostViewModel()
                                            {
                                                PostId = x.PostId,
                                                Name = x.Name,
                                                Description = x.Description,
                                                Img = x.Img,
                                                CreateDate = x.CreateDate,
                                                Alias = x.Alias

                                            }).FirstOrDefault();
            listResult.Slides = _db.Slides.Where(x => x.Deleted == false && x.Status == true)
                                            .Select(x => new SlideViewModel()
                                            {
                                                Img = x.Img
                                            }).ToList();
            return View(listResult);
        }

        public ActionResult Menu()
        {
            try
            {
                var result = _db.Menus.Where(x=>x.Status == true).Select(x => new MenuViewModel()
                {
                    Id = x.MenuId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Link = x.Link,
                    DisplayOrder = x.DisplayOrder,
                    URL = x.URL
                    
                }).ToList();

                List<MenuViewModel> listResult = new List<MenuViewModel>();

                foreach (var item in result)
                {
                    if (item.ParentId == 0)
                    {
                        foreach (var menu in result)
                        {
                            if (menu.ParentId == item.Id)
                            {
                                item.SubItem.Add(menu);
                            }

                        }
                        listResult.Add(item);
                    }
                    else
                    {
                        foreach (var sub in result)
                        {
                            if (sub.ParentId == item.Id)
                            {
                                item.SubItem.Add(sub);
                            }
                        }
                    }
                }
                var listMenu = listResult.ToList().OrderBy(x => x.DisplayOrder).ToList();
                return PartialView(listMenu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }        
        public ActionResult Footer()
        {
            var result = _db.WebInfos.SingleOrDefault();
            return PartialView(result);
        }
        public ActionResult _TopHeader()
        {
            var result = _db.WebInfos.SingleOrDefault();
            return PartialView(result);
        }
        public ActionResult Image()
        {
            var result = _db.Images.Select(x => new ImageViewModel()
            {
                Img = x.Img
            }).ToList();
            return PartialView(result);
        }

        public ActionResult Partner()
        {
            var result = _db.Partners.Where(x=>x.Status == true).Select(x => new PartnerViewModel()
            {
                Img = x.Img,
                Url = x.Url
            }).ToList();
            return PartialView(result);
        }
    }
}