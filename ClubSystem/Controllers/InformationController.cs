using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using ClubSystem.Models;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using Newtonsoft.Json;
namespace ClubSystem.Controllers

{
    public class InformationController : Controller
    {
        //
        // GET: /Information/
        #region 创建新闻
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /News/ 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection fc)
        {

            //string content = Request.Form["editor"];
            //return View();
            ////var title = SaveTitle(json);

            string data = Request.Form["title"];
            string content = fc["editor"];
            System.DateTime starttime = new System.DateTime();
            starttime = DateTime.Now;
            if (data == "")
            {
                ViewData["back_news"] = "标题为空，请重新输入";
                return View();
            }
            else if (content == "")
            {
                ViewData["back_news"] = "内容为空，请重新输入";
                return View();
            }
            else
            {
                try
                {
                    using (var db = new ClusDBEntities())
                    {
                        var news = new t_f_news()
                        {
                            Title = data,
                            Content = content,
                            EditTime = starttime,
                            isDel = false
                        };
                        db.t_f_news.Add(news);
                        db.SaveChanges();
                        ViewData["back_news"] = "保存成功！";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["back_news"] = "保存失败";
                    return View();
                }
            }
        }
        #endregion
        #region 新闻列表
        public ActionResult NewsList()
        {
            return View();
        }
        #endregion
        #region 111
        public ActionResult CreatNews()
        {
            return View();
        }
        
        #endregion
        #region 编辑新闻
        public ActionResult EditNews(string id)
        {
            ClusDBEntities db = new ClusDBEntities();
            List<SelectListItem> recuit = new List<SelectListItem>();
            decimal newsid = decimal.Parse(id);
            t_f_news dd = db.t_f_news.Where(s => s.ID == newsid).First();
            ViewData["newsid"] = dd.ID;//编号ID
            ViewData["time"] = dd.EditTime;//时间
            ViewData["title1"] = dd.Title;//标题
            ViewData["content"] = dd.Content;//内容
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(FormCollection fc)
        {

            //string content = Request.Form["editor"];
            //return View();
            ////var title = SaveTitle(json);
            decimal newsid = Decimal.Parse(Request.Form["newsid"]);
            string data = Request.Form["title"];
            string content = fc["editor"];
            System.DateTime starttime = new System.DateTime();
            starttime = DateTime.Now;
            if (data == "")
            {
                ViewData["back_news"] = "标题为空，请重新输入";
                return View();
            }
            else if (content == "")
            {
                ViewData["back_news"] = "内容为空，请重新输入";
                return View();
            }
            else
            {
                try
                {
                    using (var db = new ClusDBEntities())
                    {

                        var recruit = db.t_f_news.FirstOrDefault(s => s.ID == newsid);
                        recruit.Title = data;
                        recruit.Content = content;
                        recruit.EditTime = starttime;
                        db.SaveChanges();
                        ViewData["back_news"] = "保存成功！";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["back_news"] = "保存失败";
                    return View();
                }
            }
        }
        #endregion
        #region 删除新闻
        public ActionResult DeleteNews(string id)
        {
            var db = new ClusDBEntities();
            decimal newsid = decimal.Parse(id);
            var q = db.t_f_news.FirstOrDefault(m => m.ID == newsid);
            q.isDel = true;
            db.SaveChanges();
            var data = from a in db.t_f_news
                       where a.isDel == false
                       orderby a.ID ascending
                       select new NewsCom
                       {
                           ID = a.ID,
                           EditTime = a.EditTime,
                           Title = a.Title,
                           Content = a.Content,
                       };
            return Json(new GridModel()
            {
                Data = data,
                Total = data.Count()
            });
        }
        #endregion
        #region 新闻通知
        public ActionResult Newss()
        {
            return View();
        }
        public ActionResult NewsContent(string id)
        {
            ClusDBEntities db = new ClusDBEntities();
            decimal pid1 = decimal.Parse(id);
            t_f_news dd = db.t_f_news.Where(s => s.ID == pid1).First();
            ViewData["did"] = dd.ID;//编号
            ViewData["endtime"] = dd.EditTime;//时间
            ViewData["content"] = dd.Content;//内容
            ViewData["TTitle"] = dd.Title;//标题
            return View();
        }
        #endregion
    }
}