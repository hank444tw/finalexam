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
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult MemberCenter()
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

        [HttpPost]
        public ActionResult Signup(Member mer,string Mem_id)
        {
            var mid = db.Member.Where(m => m.Mem_id == Mem_id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (mid == null)
                {
                    db.Member.Add(mer);
                    db.SaveChanges();
                    ViewBag.Message = "註冊成功 ! 請重新登入";
                    return RedirectToAction("Signin");
                }
                else
                {
                    ViewBag.Message="已經有重複的帳號";
                    
                }
            }
            return View(mer);
        }


        public ActionResult Signout()
        {
            Session["ChangeLayout"] = null;
            return RedirectToAction("Index");
        }
    }
}