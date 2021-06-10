using FEE.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Areas.Admin.Controllers
{
    public class BackUpController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated == false)
            {

                return RedirectToAction("Login", "Home");
            }
            else
            {
                string kq = "";
                try
                {
                    string strCon = ConfigurationManager.ConnectionStrings["FEEBackUp"].ToString();
                    string databasename = "FEE";
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/backup/"), databasename + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".bak");
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    string sqlBackup = "BACKUP DATABASE [" + databasename + "] TO DISK='" + path + "'";
                    SqlConnection con = new SqlConnection(strCon);
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sqlBackup, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    kq = path.Substring(path.IndexOf("UploadedFiles") - 1);
                    Notification.set_flash("Lưu thành công!", "success");
                    byte[] fileBytes = GetFile(path);
                    return File(fileBytes, "application/octet-stream", databasename + "-" + DateTime.Now.ToString("dd-MM-yyyy")+".bak");
                }
                catch (Exception ex)
                {
                    Notification.set_flash("Lưu thất bại!", "warning");
                    kq = "Err"; 
                }
                return RedirectToAction("Index","Home");
            }
        }
        private byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}