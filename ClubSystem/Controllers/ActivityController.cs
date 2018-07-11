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
using ClubSystem.Web.Models;
using System.IO;
namespace ClubSystem.Controllers
{
    public class ActivityController : Controller
    {
        //
        // GET: /Activity/
        public ActionResult Index()
        {
            return View();
        }
        #region 活动申请
        public ActionResult ApplyActivity()
        {
            return View();
        }
        [HttpPost]//保存new user
        public string SaveActivity(string json)
        {
            JsonObject obj = new JsonObject(json);
            string clubtittle = obj["clubtittle"].Value.ToString();
            string clubtime = obj["clubtime"].Value.ToString();
            string clubplace = obj["clubplace"].Value.ToString();
            string cluboption1 = obj["cluboption1"].Value.ToString();
            string clubform = obj["clubform"].Value.ToString();
            System.DateTime com_starttime = new System.DateTime();
            com_starttime = DateTime.Now;
            try
            {
                using (var db = new ClusDBEntities())
                {
                    t_f_User user = Session["user"] as t_f_User;
                    int userid = user.ID;//社团ID
                    if (userid != null)
                    {
                        var activity = new t_f_activity()
                        {
                            club_ID = userid,//社团ID
                            tittle = clubtittle,//活动主题
                            option2 = clubtime,//活动时间
                            place = clubplace,//活动地点
                            option1  = cluboption1,//活动经费
                            ac_form = clubform,//活动内容
                            starttime = com_starttime,//申请时间
                            option3 = "审核中……"
                        };
                        db.t_f_activity.Add(activity);
                        db.SaveChanges();
                        return "success";
                    }
                    else
                    {
                        return "existed";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        #region 管理员活动列表 +page
        public ActionResult AdminAcList()
        {
            return View();
        }
        public ActionResult AdminLiPage(string id)
        {
            ClusDBEntities db = new ClusDBEntities();
            int pid1 = int.Parse(id);
            var dataa = (from a in db.t_f_activity
                       join b in db.t_f_User on a.club_ID equals b.ID
                       where a.ID == pid1
                       select new Activity
                       {
                           acid = a.ID,//活动ID
                           actittle = a.tittle,//活动主题
                           acclub = b.Name,//社团名称
                           clubtime = a.option2,//活动时间
                           clubplace = a.place,//活动地点
                           lastmoney = b.option1, // 社团剩余经费
                           money = a.option1,//活动经费
                           sarttime = a.starttime,//申请时间
                           option3 = a.option3,//活动状态
                           clubform = a.ac_form,//活动内容
                       }).First();
            ViewData["acid"] = dataa.acid;
            ViewData["actittle"] = dataa.actittle;
            ViewData["acclub"] = dataa.acclub;
            ViewData["clubtime"] = dataa.clubtime;
            ViewData["clubplace"] = dataa.clubplace;
            ViewData["lastmoney"] = dataa.lastmoney;
            ViewData["money"] = dataa.money;
            ViewData["sarttime"] = dataa.sarttime;
            ViewData["option3"] = dataa.option3;
            ViewData["clubform"] = dataa.clubform;
            return View();
        }
        public ActionResult PassActivity(string clubid)
        {
            var result = new ResultInfo(false);
            int clubid1 = int.Parse(clubid);
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid1);
                    q.option3 = "审核完成，待汇报经费";
                    if (db.SaveChanges() > 0)
                    {
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
            }
            return Json(result);
        }
        public ActionResult NPassActivity(string clubid, string str)
        {
            var result = new ResultInfo(false);
            int clubid1 = int.Parse(clubid);
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid1);
                    q.option3 = "审核未通过";
                    q.option4 = str;
                    if (db.SaveChanges() > 0)
                    {
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
            }
            return Json(result);
        }
        #endregion
        #region 管理员活动完成管理列表
        #endregion
        #region 社团活动列表 +page
        public ActionResult UserAcList()
        {
            return View();
        }
        public ActionResult UserLiPage(string id)
        {
            ClusDBEntities db = new ClusDBEntities();
            int pid1 = int.Parse(id);
            var dataa = (from a in db.t_f_activity
                         join b in db.t_f_User on a.club_ID equals b.ID
                         where a.ID == pid1
                         select new Activity
                         {
                             acid = a.ID,//活动ID
                             actittle = a.tittle,//活动主题
                             acclub = b.Name,//社团名称
                             clubtime = a.option2,//活动时间
                             clubplace = a.place,//活动地点
                             lastmoney = b.option1, // 社团剩余经费
                             money = a.option1,//活动经费
                             sarttime = a.starttime,//申请时间
                             option3 = a.option3,//活动状态
                             clubform = a.ac_form,//活动内容
                             unpass = a.option4//未通过原因
                         }).First();
            ViewData["acid"] = dataa.acid;
            ViewData["actittle"] = dataa.actittle;
            ViewData["acclub"] = dataa.acclub;
            ViewData["clubtime"] = dataa.clubtime;
            ViewData["clubplace"] = dataa.clubplace;
            ViewData["lastmoney"] = dataa.lastmoney;
            ViewData["money"] = dataa.money;
            ViewData["sarttime"] = dataa.sarttime;
            ViewData["option3"] = dataa.option3;
            ViewData["clubform"] = dataa.clubform;
            ViewData["unpass"] = dataa.clubform;
            return View();
        }
        public string SaveUActivity(string json)
        {
            var result = new ResultInfo(false);
            JsonObject obj = new JsonObject(json);
            int acid = int.Parse(obj["clubid"].Value.ToString());
            string clubtittle = obj["clubtittle"].Value.ToString();
            string clubtime = obj["clubtime"].Value.ToString();
            string clubplace = obj["clubplace"].Value.ToString();
            string cluboption1 = obj["cluboption1"].Value.ToString();
            string clubform = obj["clubform"].Value.ToString();
            System.DateTime com_starttime = new System.DateTime();
            com_starttime = DateTime.Now;
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == acid);
                    q.tittle = clubtittle;//活动主题
                    q.option2 = clubtime;//活动时间
                    q.place = clubplace;//活动地点
                    q.option1  = cluboption1;//活动经费
                    q.ac_form = clubform;//活动内容
                    q.starttime = com_starttime;//申请时间
                    q.option3 = "审核中……";
                    if(db.SaveChanges()>0)
                    {
                        return "success";
                    }else
                    {
                        return "existed";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult CancelActivity(string clubid)
        {
            var result = new ResultInfo(false);
            int clubid1 = int.Parse(clubid);
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid1);
                    q.option3 = "活动取消";
                    if (db.SaveChanges() > 0)
                    {
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
            }
            return Json(result);
        }
        #endregion
        #region 添加消费详单
        public ActionResult AddMoneyList(string id)
        {
            ClusDBEntities db = new ClusDBEntities();
            int pid1 = int.Parse(id);
            var dataa = (from a in db.t_f_activity
                         where a.ID == pid1
                         select new Activity
                         {
                             acid = a.ID,//活动ID
                             actittle = a.tittle,//活动主题
                             money = a.option1,//活动经费
                         }).First();
            ViewData["acid1"] = dataa.acid;
            ViewData["actittle1"] = dataa.actittle;
            ViewData["money1"] = dataa.money;
            return View();
        }
        [HttpPost]
        public string SaveUMoney(string json)
        {
            var result = new ResultInfo(false);
            JsonObject obj = new JsonObject(json);
            int clubid = int.Parse(obj["clubid"].Value.ToString());
            string factmoney = obj["factmoney"].Value.ToString();
            string com_content = obj["com_content"].Value.ToString();
            System.DateTime com_starttime = new System.DateTime();
            com_starttime = DateTime.Now;
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var finance = new t_f_finance()
                    {
                        price = factmoney,//消费金额
                        tittle = com_content,//文件位置
                        fi_time = com_starttime, //申请时间
                        fi_type = clubid  //活动id
                    };
                    db.t_f_finance.Add(finance);
                    if (db.SaveChanges() > 0)
                    {
                        var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid);
                        q.option3 = "待转账";
                        if(db.SaveChanges()>0)
                            return "success";
                        else
                            return "existed";
                    }
                    else
                    {
                        return "existed";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #region 文件传入服务器数据库
        [HttpPost]
        ////案例
        public ActionResult File_Save(HttpPostedFileBase file_upload)
        {
            //获取文件的类型
            string filename = Path.GetFileName(file_upload.FileName);
            string filetype = System.IO.Path.GetExtension(filename);
            //string filetype = "." + file_upload.ContentType.Split('/')[1];
            //获取文件的路径
            string fileName = file_upload.FileName;

            //判断格式
            if (filetype == ".xlsx" || filetype == ".xlsm" || filetype == ".xltx"|| filetype == ".xltm" || filetype == ".xlsb"|| filetype == ".xlam")
            {
                //转换只取文件名，去掉路径。
                if (fileName.LastIndexOf("\\") > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                }
                //保存到相对路径下。
                bool abc = Directory.Exists(Server.MapPath("~/Upload"));
                if (abc == false)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Upload"));
                }
                file_upload.SaveAs(Server.MapPath("~/Upload/" + fileName));
                //以下代码是将路径保存到数据库。
                string FilePath = "../../Upload/" + fileName;
                //插入到数据库
                try
                {
                    using (var db = new ClusDBEntities())
                    {
                        return Content("1|" + FilePath + "|上传成功!|");
                    }
                }
                catch (Exception ex)
                {
                    return Content("0|网络繁忙,请稍后在试！");
                }
            }
            else
            {
                return Content("0|请选择Excel格式的文件！");
            }
        }
        #endregion
        #endregion
        #region 管理员 已转账+未通过汇报审核驳回
        public ActionResult HadMoney(string clubid)
        {
            var result = new ResultInfo(false);
            int clubid1 = int.Parse(clubid);
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid1);
                    q.option3 = "活动成功结束";
                    if (db.SaveChanges() > 0)
                    {
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
            }
            return Json(result);
        }
        public ActionResult BackMoney(string clubid,string str)
        {
            var result = new ResultInfo(false);
            int clubid1 = int.Parse(clubid);
            try
            {
                using (var db = new ClusDBEntities())
                {
                    var q = db.t_f_activity.FirstOrDefault(m => m.ID == clubid1);
                    q.option3 = "审核完成，待汇报经费";
                    q.option5 = str;
                    if (db.SaveChanges() > 0)
                    {
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
            }
            return Json(result);
        }
        
        public ActionResult WaitMoney()
        {
            return View();
        }
        #endregion
    }
}