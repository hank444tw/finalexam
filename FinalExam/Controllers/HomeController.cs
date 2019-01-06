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
<<<<<<< HEAD

    public int pagesize = 6; //要顯示的資料數量
        string memacc = "";
=======
    public int pagesize = 6; //要顯示的資料數量
>>>>>>> 75b82126df7ca26ccfb9d173081c997c00e569d6

        public ActionResult Index()
        {
            ViewBag.Title = "首頁";
            return View();
        }
        public ActionResult MemberManage()
        {
            string memacc = Session["memacc"].ToString();
            var result = db.Member;
            string show = "";
            foreach (var m in result)
            {
                show += "<tr>";
                show += "<th>" + m.Name + "</th>";
                show += "<th>" + m.Mem_id + "</th>";
                show += "<th>" + "************" + "</th>";
                show += "<th>" + m.Phone + "</th>";
                show += "</tr>";
            }
            ViewData["MemberData"] = show;
            return View();
        }
        [HttpPost]
        public ActionResult MemberManage(Member mr)
        {
            string memacc = Session["memacc"].ToString();
            var result = db.Member.Where(m => m.Mem_id == memacc).FirstOrDefault();
            result.Name = mr.Name;
            result.Mem_password = mr.Mem_password;
            result.Phone = mr.Phone;
            db.SaveChanges();
            Session["memacc"] = "";
            return RedirectToAction("Signin");
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

        public ActionResult ShoppingCart()
        {
            //只顯示那個會員所加入購物車的商品
            int id = (int)Session["MemberId"];
            var todo = db.Order.Where(m => m.MemberId == id && m.CheckSell == "Not").ToList();
            return View(todo);
        }

        [HttpPost]
        public ActionResult ShoppingCart(int TotalMoney)
        {
            BigOrder bigorder = new BigOrder();
            int id = (int)Session["MemberId"];
            var member = db.Member.Where(m => m.Id == id).FirstOrDefault();
            var todo = db.Order.Where(m => m.MemberId == id && m.CheckSell == "Not").ToList();
            string OId = GetRandomStringByGuid();
            foreach (var item in todo)
            {
                item.CheckSell = OId;
            }
            bigorder.OId = OId;
            bigorder.MemberId = id;
            bigorder.Money = TotalMoney;
            bigorder.BuyDate = DateTime.Now.ToString();
            bigorder.MamberName = member.Name;
            db.BigOrder.Add(bigorder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyOrder()
        {
            int id = (int)Session["MemberId"];
            var todo = db.BigOrder.Where(m => m.MemberId == id).ToList();
            return View(todo);
        }

        public ActionResult SeeBuyOrder(string id)
        {
            var todo = db.Order.Where(m => m.CheckSell == id).ToList();

            return View(todo);
        }

        public ActionResult OrderManage()
        {
            var todo = db.BigOrder.ToList();
            return View(todo);
        }

        public ActionResult DeleteShoppingOrder(int id)
        {
            var todo = db.Order.Where(m => m.Id == id).FirstOrDefault();
            db.Order.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("ShoppingCart");
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
            Session["memacc"] = "";
            return RedirectToAction("Signin");
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
            Session["EditProductId"] = id;
            TwoModelProduct todo = new TwoModelProduct();
            todo.Product = db.Product.Where(m => m.PId == id).FirstOrDefault();
            todo.ProductImage = db.ProductImage.Where(m => m.PId == id).ToList();
            return View(todo);
        }

        [HttpPost]
        public ActionResult EditProduct(string PId,string ProductName,int ProductMoney, string ProductIntrodution, IEnumerable<HttpPostedFileBase> fileList)
        {
            var product = db.Product.Where(m => m.PId == PId).FirstOrDefault();
            var productimage = db.ProductImage.Where(m => m.PId == PId).ToArray();
            int ImageAmount = db.ProductImage.Where(m => m.PId == PId).Count(); //取得先前上傳圖片數量
            int GG = 0;
            int count = 0;
            ProductImage productimageNew = new ProductImage();
            foreach (var item in fileList)
            {
                if (item == null || item.ContentLength == 0) //判斷檔案是否為空的
                {
                    count++;
                    continue;
                }
                if(count <= ImageAmount-1)
                {
                    var FolderPath = Server.MapPath("~/Image/" + PId); //圖片資料夾實體位置
                    var path = Path.Combine(FolderPath, productimage[count].ImageName + " .png"); //圖片檔案位置
                    System.IO.File.Delete(path); //刪除圖片檔案

                    string fileName = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
                    item.SaveAs(Path.Combine(FolderPath, fileName + " .png"));

                    productimage[count].ProductName = ProductName;
                    productimage[count].ImageName = fileName;
                    if(count == 0)
                    {
                        product.OneImageName = fileName;
                    }
                    db.SaveChanges();
                    count++;
                }
                else
                {
                    var FolderPath = Server.MapPath("~/Image/" + PId); //圖片資料夾實體位置
                    string fileName = GetRandomStringByGuid(); //跳到取亂碼的GetRandomStringByGuid方法
                    item.SaveAs(Path.Combine(FolderPath, fileName + " .png"));
                    productimageNew.PId = PId;
                    productimageNew.ProductName = ProductName;
                    productimageNew.ImageName = fileName;
                    db.ProductImage.Add(productimageNew);
                    db.SaveChanges();
                }
            }
            product.ProductName = ProductName;
            product.ProductMoney = ProductMoney;
            product.ProductIntrodution = ProductIntrodution;
            db.SaveChanges();
            return RedirectToAction("ProductManage");
        }

        public ActionResult DeleteImage(string ImageName)
        {
            var todo = db.ProductImage.Where(m => m.ImageName == ImageName).FirstOrDefault();
            db.ProductImage.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("EditProduct","Home",new {id = (string)Session["EditProductId"]});
        }

        public ActionResult DeleteProduct(string id)
        {
            var todo = db.Product.Where(m => m.PId == id).FirstOrDefault();
            var todo2 = db.ProductImage.Where(m => m.PId == id).ToList();

            var FolderPath = Server.MapPath("~/Image/" + id); //圖片資料夾實體位置
            System.IO.Directory.Delete(FolderPath, true); //加了true,資料夾連同裡面圖片刪光光，爽!!

            db.Product.Remove(todo);
            foreach(var item in todo2)
            {
                db.ProductImage.Remove(item);
            }
            db.SaveChanges();
            
            return RedirectToAction("ProductManage");
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