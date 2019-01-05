using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalExam.Models; //導入Model模型
using System.IO;  //儲存、刪除本機資料要用到

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

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(Product product,HttpPostedFileBase file)
        {
            int fid = db.Product.Max(m => m.Id) + 1 ; //找到當前最大的Id值並+1,見鬼了不用Value
            string pathimage = Server.MapPath("~/Image/" + fid.ToString());
            System.IO.Directory.CreateDirectory(pathimage); //新增資料夾

            if (file == null || file.ContentLength == 0)//判斷檔案是否為空的
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 

            string fileName = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
            file.SaveAs(Path.Combine(pathimage, fileName + " .png"));
            product.ImageName = fileName;
            db.Product.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static string GetRandomStringByGuid()  //使用Guid產生亂碼
        {
            var str = Guid.NewGuid().ToString().Replace("-", ""); //將"-"字號去掉
            return str;
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