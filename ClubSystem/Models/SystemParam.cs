using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubSystem.Models
{
    public class SystemParam
    {
        /// <summary>
        ///  获取当前登录用户
        /// </summary>
        public static t_f_User CurrentUser
        {
            get { return (HttpContext.Current.Session["user"] as t_f_User); }
        }
        public static string EnvironmentPath
        {
            get { return HttpContext.Current.Server.MapPath("~"); }
        }
    }
}