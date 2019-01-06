using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalExam.Models; //導入Model模型
using System.IO;  //儲存、刪除本機資料要用到
using PagedList;

namespace FinalExam.Controllers
{ 
    public class HomeController : Controller
    {
    DBFinalExamEntities db = new DBFinalExamEntities();
<<<<<<< .merge_file_a17228
    public int pagesize = 6; //要顯示的資料數量
        string memacc = "";
=======
>>>>>>> .merge_file_a14496
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

        public ActionResult ProductPage(string Id)
        {
            TwoModelProduct todo = new TwoModelProduct(); //自己新增的類別，把兩個Model放進去
            todo.Product = db.Product.Where(m => m.PId == Id).FirstOrDefault();
            todo.ProductImage = db.ProductImage.Where(m => m.PId == Id).ToList();
            return View(todo);
        }

        [HttpPost]
        public ActionResult ProductPage(string ProductName, int ProductMoney, int Amount)
        {
            if (Session["MemberId"] == null) //如果沒登入，就給我先去登入
            {
                return RedirectToAction("Signin");
            }
            Order order = new Order();
            order.MemberId = (int)Session["MemberId"];
            order.ProductName = ProductName;
            order.Amount = Amount;
            order.ProductMoney = ProductMoney;
            order.CheckSell = "Not"; //還未結帳
            db.Order.Add(order);
            db.SaveChanges();
            return RedirectToAction("AllProduct");
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

        public ActionResult ProductManage()
        {
            var todo = db.Product.ToList();
            return View(todo);
        }

        public ActionResult AllProduct(int Page = 1) //初始頁碼為1
        {
            var Product = db.Product.ToList();
            int CurrentPage = Page < 1 ? 1 : Page;
            var Result = Product.ToPagedList(CurrentPage, pagesize);
            return View(Result);
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
            product.PId = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
            string pathimage = Server.MapPath("~/Image/" + product.PId);
            string path = Server.MapPath("~/Image/");
            System.IO.Directory.CreateDirectory(pathimage); //新增該產品的圖片資料夾

            int GG = 0;
            foreach (var item in fileList)
            {
                ProductImage productimage = new ProductImage();
                Product product2 = new Product();
                if (item == null || item.ContentLength == 0) //判斷檔案是否為空的
                {
                    GG++; //GG為3時代表都沒上傳圖片
                    if (GG == 3)
                    {
                        //放張"此產品尚未有圖片"的圖片進去資料夾
                        System.IO.File.Copy(Path.Combine(path, "NoImage .png"), Path.Combine(pathimage, "NoImage .png"));
                        productimage.ImageName = "NoImage";
                        productimage.PId = product.PId;
                        productimage.ProductName = product.ProductName;
                        db.ProductImage.Add(productimage);
                        db.SaveChanges();
                    }
                    continue;
                }
                string fileName = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
                item.SaveAs(Path.Combine(pathimage, fileName + " .png"));
                productimage.PId = product.PId;  //圖片存進ProductImage資料表,PId為產品的PId
                productimage.ProductName = product.ProductName;
                productimage.ImageName = fileName;
                db.ProductImage.Add(productimage);
                db.SaveChanges();
            }
            var todo = db.ProductImage.Where(m => m.PId == product.PId).FirstOrDefault(); //找第一張圖片當封面
            product.OneImageName = todo.ImageName;
            db.Product.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditProduct(string id)
        {
            TwoModelProduct todo = new TwoModelProduct();
            todo.Product = db.Product.Where(m => m.PId == id).FirstOrDefault();
            todo.ProductImage = db.ProductImage.Where(m => m.PId == id).ToList();
            return View(todo);
        }

        [HttpPost]
        public ActionResult EditProduct()
        {
            
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
            Session["MemberId"] = member.Id;
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