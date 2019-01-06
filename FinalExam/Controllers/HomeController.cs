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
            string memacc = Session["memacc"].ToString() ;
            var result = db.Member.Where(m => m.Mem_id.Contains(memacc));
            string show = "";
            foreach(var m  in result)
            {
                show += "<tr>";
                show += "<th>" + m.Name + "</th>";
                show += "<th>" + m.Mem_id + "</th>";
                show += "<th>" + "************"+ "</th>";
                show += "<th>" + m.Phone + "</th>";
                show += "</tr>";
            }
            ViewData["MemberData"] = show;
            return View();
        }

        public ActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MemberCenter(Member mr)
        {
            string memacc = Session["memacc"].ToString();
            var result = db.Member.Where(m => m.Mem_id == memacc).FirstOrDefault();
            result.Name = mr.Name;
            result.Mem_password = mr.Mem_password;
            result.Phone = mr.Phone;
            db.SaveChanges();
            return View("MemberCenter");
        }



        [HttpPost]
        public ActionResult CreateProduct(Product product, IEnumerable<HttpPostedFileBase> fileList)
        {
            int fid = db.Product.Max(m => m.Id) + 1 ; //找到當前最大的ProductId值並+1,見鬼了不用Value
            string pathimage = Server.MapPath("~/Image/" + fid.ToString());
            string path = Server.MapPath("~/Image/");
            System.IO.Directory.CreateDirectory(pathimage); //新增該產品的圖片資料夾

            int GG = 0;
            foreach (var item in fileList)
            {
                ProductImage productimage = new ProductImage();
                if (item == null || item.ContentLength == 0) //判斷檔案是否為空的
                {
                    GG++; //GG為3時代表都沒上傳圖片
                    if(GG == 3)
                    {
                        //放張"此產品尚未有圖片"的圖片進去資料夾
                        System.IO.File.Copy(Path.Combine(path, "NoImage.png"), Path.Combine(pathimage, "NoImage.png"));
                        productimage.ImageName = "NoImage.png";
                        productimage.PId = fid;
                        productimage.ProductName = product.ProductName;
                        db.ProductImage.Add(productimage);
                        db.SaveChanges();
                    }
                    continue;
                }
                string fileName = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
                item.SaveAs(Path.Combine(pathimage, fileName + " .png"));
                productimage.PId = fid;  //圖片存進ProductImage資料表,PId為產品的Id
                productimage.ProductName = product.ProductName;
                productimage.ImageName = fileName;
                db.ProductImage.Add(productimage);
                db.SaveChanges();
            }
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
            Session["memacc"]= member.Mem_id;
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
                    return RedirectToAction("Signin");
                }
                else
                {
                   
                    ViewData["Message"]= "sameacc";
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