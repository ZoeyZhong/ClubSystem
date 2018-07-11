using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClubSystem.Models;
using System.Text;
using System.Data;

namespace ClubSystem.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Logon", "Home");
            return View();
        }
        #region 注销
        public void LogOff()//注销
        {
            Session.Abandon();  //把当前Session对象删除了
            Session.Clear();  //把Session对象中的所有项目都删除了
            Response.Redirect("./Index");
        }
        #endregion
        #region 登录
        public ActionResult Logon()
        {
            return View();
        }
        [HttpPost]
        public string LogOn(string handno, string pwd)
        {
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var user = db.t_f_User.Where(p => p.HandNo == handno && p.Password == pwd && p.IsDelete == false).FirstOrDefault();
                    if (user != null)
                    {
                        Session["user"] = user;
                        return "success";
                    }
                    else
                    {
                        return "密码有误！";
                    }
                }
            }
            catch (Exception ex)
            {
                return "用户名有误！" + ex.Message;
            }
        }
        #endregion
    }
}