using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.Library
{
    public static class Notification
    {
        public static bool has_flash()
        {
            if (HttpContext.Current.Application["Notification"].Equals(""))
            {
                return false;
            }
            return true;
        }
        public static void set_flash(String mgs, String mgs_type)
        {
            ModelNotification tb = new ModelNotification();
            tb.mgs = mgs;
            tb.mgs_type = mgs_type;

            HttpContext.Current.Application["Notification"] = tb;
        }
        public static ModelNotification get_flash()
        {

            ModelNotification Notifi = (ModelNotification)HttpContext.Current.Application["Notification"];
            HttpContext.Current.Application["Notification"] = "";
            return Notifi;
        }

    }
}