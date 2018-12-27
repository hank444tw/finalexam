using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalExam.Models; //導入Model模型

namespace FinalExam.Controllers
{ 
    public class HomeController : Controller
    {
    DBFinalExamEntities db = new DBFinalExamEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string Mem_id,string Mem_password)
        {
            var member = db.Member.Where(m => m.Mem_id == Mem_id && m.Mem_password == Mem_password).FirstOrDefault();
            if(member == null)
            {
                ViewBag.Message = "帳號或密碼錯誤";
                return View();
            }

            Session["Welcome"] = member.Name + " 你好!"; //姓名歡迎詞
            if(member.Id == 1)  //判斷是否為管理員
            {
                Session["ChangeLayout"] = "Administrator";
            }
            else
            {
                Session["ChangeLayout"] = "Member";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Signout()
        {
            Session["ChangeLayout"] = null;
            return RedirectToAction("Index");
        }
    }
}