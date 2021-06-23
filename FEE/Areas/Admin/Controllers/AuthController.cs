using FEE.Authorize;
using FEE.Dtos;
using FEE.Enums;
using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FEE.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private FEEDbContext db = new FEEDbContext();
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Session.Clear();
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public void setCookie(string username, bool rememberme = false, int role = 0)
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               rememberme,
                               role.ToString()
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var exist = db.Users.Any(x => x.Username == model.Username);
                            
                if (exist)
                {
                    var user = db.Users.Where(e => e.Username.Equals(model.Username)).First();
                    if (user != null)
                    {
                        if (user.Password == XString.ToMD5(model.Password) && user.Status == (int)UserStatus.Activated)
                        {
                            setCookie(user.Username, model.RememberMe, user.RoleId);
                            var userSession = new UserSession();
                            userSession.Id = user.Id;
                            userSession.Name = user.Name;
                            userSession.RoleId = user.RoleId;
                            userSession.Username = user.Username;
                            userSession.DepartmentId = user.DepartmentId;

                            Session.Add("USER", userSession);
                            Session.Add("PERMISSION", AuthPermission.GetProfileService(user.Id));
                            if (ReturnUrl != null)
                                return Redirect(ReturnUrl);
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                        return View();

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.oldpassword == model.password)
                {
                    ModelState.AddModelError("", "Mật khẩu mới không được trùng mật khẩu cũ !");
                    return View();
                }
                else
                {
                    User user = db.Users.Where(e => e.Username == User.Identity.Name).First();
                    if (user != null)
                    {
                        user.Password = XString.ToMD5(model.password);
                        user.UpdateDate = DateTime.Now;
                        user.UpdateBy = user.Id;
                        db.SaveChanges();
                        Notification.set_flash("Lưu thành công!", "success");
                        return View();
                    }
                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult RePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RePassword(RePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Views/Mail/G_Pass.html"));
                string pass = Guid.NewGuid().ToString().Substring(5, 10);
                var acc = db.Users.Where(x=>x.Username == model.Username).FirstOrDefault();
                if(acc == null)
                {
                    Notification.set_flash("Thông tin bạn cung cấp không đúng!", "warning");
                    return View("RePassword");
                }
                else
                {
                    acc.Password = XString.ToMD5(pass);
                    acc.UpdateDate = DateTime.Now;
                    acc.UpdateBy = acc.Id;
                    db.SaveChanges();
                    content = content.Replace("{{password}}", pass);
                    string subject = "Phản hồi liên hệ từ Khoa Điện - Điện tử, trường Đại học Sư phạm Kỹ thuật Hưng Yên";
                    new MailHelper().SendMail(acc.Email, subject, content);
                    Notification.set_flash("Đã gửi mật khẩu tạm thời tới email của bạn!", "success");
                    return View("Login");
                }
            }
            Notification.set_flash("Trả lời thất bại!", "warning");
            return View();
        }
    }
}