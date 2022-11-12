using PagedList;
using prjShoppingCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace prjShoppingCar.Controllers
{

    [Authorize]  //指定Member控制器所有的動作方法必須通過授權才能執行。
    public class MemberController : Controller
    {
        //建立可存取dbShoppingCar.mdf 資料庫的dbShoppingCarEntities 類別物件db
        dbShoppingCarEntities db = new dbShoppingCarEntities();
//        dbShoppingCarAzureEntities db = new dbShoppingCarAzureEntities();
        int pageSize = 3;
        string fileName = "";
        string fUserId;

        public ActionResult Member()
        {
            string fUserId = User.Identity.Name;
            //找出未成為訂單明細的資料，即購物車內容
            var employees = db.tMember.Where(m => m.fUserId == fUserId).ToList();
            //var employees = db.tMember.ToList();
            return View(employees);
        }
        public ActionResult Edit(int fEmpId)
        {
            var employee = db.tMember
                .Where(m => m.fId == fEmpId).FirstOrDefault();
            return View(employee);
        }
        [HttpPost]
        public ActionResult Edit(tMember employee)
        {
            if (ModelState.IsValid)
            {
                var temp = db.tMember
                    .Where(m => m.fId == employee.fId)
                    .FirstOrDefault();
                temp.fName = employee.fName;
                temp.fPwd = employee.fPwd;
                temp.fUserId = employee.fUserId;
                temp.fEmail = employee.fEmail;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public ActionResult MyCommodity()
        {
            string fUserId = User.Identity.Name;

            var products = db.tProduct
                .OrderByDescending(m => m.fId).ToList();

            var productsEnd = db.tProduct
            .OrderByDescending(m => m.fId).ToList();

            foreach (var zproducts in products.ToArray())
            {
                if (zproducts.fUId != fUserId) { products.Remove(zproducts); }
            }
            var result = products;

            var resultEnd = productsEnd;

            return View(result);
        }


        public ActionResult DeleteMyCommdity(int FId)
        {
            var zproducts = db.tProduct
           .Where(m => m.fId == FId)
           .FirstOrDefault();

            db.tProduct.Remove(zproducts);
            db.SaveChanges();
            return RedirectToAction("MyCommodity");
        }

        public ActionResult SellMyCommdity(int FId)
        {
            var zproducts = db.tProduct
           .Where(m => m.fId == FId)
           .FirstOrDefault();
            zproducts.fsell = "EEEEEEEEEE";
            db.SaveChanges();

            var zOrderDetail = db.tOrderDetail.Where(m => m.fPId == zproducts.fPId).FirstOrDefault();
            var zOrder = db.tOrder.Where(m => m.fOrderGuid == zOrderDetail.fOrderGuid).FirstOrDefault();
            zOrder.fEnd = "EEEEEEEEEE";
            db.SaveChanges();
            return RedirectToAction("MyCommodity");

        }


        public ActionResult Index(string zfdepartment,string zfclass ,int page = 1)
        {
            //取得所有產品放入products
            int currentPage = page < 1 ? 1 : page;
            var products = db.tProduct
                .OrderByDescending(m => m.fId).ToList();

            //對產品列表做篩選 符合分類的才顯示
            //zCategory = zCategory;
            if (zfdepartment != null)
            {
                foreach (var zproducts in products.ToArray())
                {
                    if (zproducts.fdepartment != zfdepartment) { products.Remove(zproducts); }
                }
            }

            if (zfclass != null)
            {
                foreach (var zproducts in products.ToArray())
                {
                    if (zproducts.fclass != zfclass) { products.Remove(zproducts); }
                }
            }

            foreach (var zproducts in products.ToArray())
            {
                if (zproducts.fstate != 1) { products.Remove(zproducts); }
                if (zproducts.fsell == "EEEEEEEEEE") { products.Remove(zproducts); }
            }


            var result = products.ToPagedList(currentPage, pageSize);

            ViewBag.fdepartment = zfdepartment;
            ViewBag.fclass = zfclass;
            return View(result);
        }
        [AllowAnonymous]


        public ActionResult Create()
        {
            //ViewBag.ffaculty = ffaculty.EducationDropDownList;
            ViewBag.fdepartment = fdepartment.EducationDropDownList;
            ViewBag.fclass = fclass.EducationDropDownList;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create
            (string fPId, string fName,string fsgtxt, string ffaculty,string fclass, string fdepartment, int fPrice,int fPrice_s, string fIsbn,string fEmail, string fLINE, HttpPostedFileBase photo)
        {
            string guid = Guid.NewGuid().ToString();
            switch (fdepartment)
            {
                case "國際企業系所":
                    ffaculty = "管理學院";
                    break;
                case "風險管理與保險學系/所":
                    ffaculty = "管理學院";
                    break;
                case "會計學系/所":
                    ffaculty = "管理學院";
                    break;
                case "企業管理學系/所":
                    ffaculty = "管理學院";
                    break;
                case "財務金融學系/所":
                    ffaculty = "管理學院";
                    break;
                case "資訊管理學系/所":
                    ffaculty = "資訊學院";
                    break;
                case "資訊傳播工程學系/所":
                    ffaculty = "資訊學院";
                    break;
                case "資訊工程學系/所":
                    ffaculty = "資訊學院";
                    break;
                case "電腦與通訊工程學系/所":
                    ffaculty = "資訊學院";
                    break;
                case "電子工程學系/所":
                    ffaculty = "資訊學院";
                    break;
                case "廣播電視學系":
                    ffaculty = "傳播學院";
                    break;
                case "新聞學系":
                    ffaculty = "傳播學院";
                    break;
                case "廣告暨策略行銷學系":
                    ffaculty = "傳播學院";
                    break;
                case "新媒體暨傳播管理學系/所":
                    ffaculty = "傳播學院";
                    break;
                case "觀光事業學系 /所":
                    ffaculty = "觀光學院";
                    break;
                case "休閒遊憩管理學系":
                    ffaculty = "觀光學院";
                    break;
                case "餐旅管理學系":
                    ffaculty = "觀光學院";
                    break;
                case "應用中國文學系/所":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "應用英語學系/所":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "應用日語學系/所":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "英語教學中心":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "教育研究所":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "華語文教學學系":
                    ffaculty = "教育暨應用語文學院";
                    break;
                case "商業設計學系/所":
                    ffaculty = "設計學院";
                    break;
                case "商品設計學系/所":
                    ffaculty = "設計學院";
                    break;
                case "數位媒體設計學系/所":
                    ffaculty = "設計學院";
                    break;
                case "都市規劃與防災學系/所":
                    ffaculty = "設計學院";
                    break;
                case "建築學系/所":
                    ffaculty = "設計學院";
                    break;
                case "動漫文創設計學士學位學程":
                    ffaculty = "設計學院";
                    break;
                case "公共事務學系/所":
                    ffaculty = "社會科學院";
                    break;
                case "諮商與工商心理學系/所":
                    ffaculty = "社會科學院";
                    break;
                case "犯罪防治學系/所":
                    ffaculty = "社會科學院";
                    break;
                case "國際事務研究所":
                    ffaculty = "國際學院";
                    break;
                case "國際事務與外交學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "國際企業與貿易學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "新聞與大眾傳播學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "時尚與創新管理學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "旅遊與觀光學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "資訊科技應用與管理學士學位學程":
                    ffaculty = "國際學院";
                    break;
                case "經濟與金融學系/所":
                    ffaculty = "金融科技學院";
                    break;
                case "應用統計與資料科學學系/所":
                    ffaculty = "金融科技學院";
                    break;
                case "金融科技應用學士/碩士學位學程":
                    ffaculty = "金融科技學院";
                    break;
                case "其他":
                    ffaculty = "其他";
                    break;
            }

            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    //取得圖檔名稱
                    fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine
                      (Server.MapPath("~/images"), fileName);
                    photo.SaveAs(path);
                }
            }
            string fUserId = User.Identity.Name;
            //檔案上傳
            tProduct todo = new tProduct();
            todo.fsgtxt = fsgtxt;
            todo.fclass = fclass;
            todo.fisbn = fIsbn;
            todo.fPId = guid;
            todo.fName = fName;
            todo.ffaculty = ffaculty;
            todo.fdepartment = fdepartment;
            todo.fPrice = fPrice;
            todo.fPrice_s = fPrice_s;
            todo.fImg = fileName;
            todo.fstate = 1;
            todo.fEmail = fEmail;
            todo.fLine = fLINE;
            todo.fUId = fUserId;
            db.tProduct.Add(todo);
            
            db.SaveChanges();

            todo = db.tProduct
            .Where(m => m.fPId == todo.fPId && m.fName == fName && m.fdepartment == fdepartment)
            .FirstOrDefault();
            todo.fcId = Convert.ToString(todo.fId);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]


        //Get:Member/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();   // 登出
            return RedirectToAction("Login", "Home");
        }

        //Get:Member/ShoppingCar
        public ActionResult ShoppingCar()
        {
            //取得登入會員的帳號並指定給fUserId
            string fUserId = User.Identity.Name;
            //找出未成為訂單明細的資料，即購物車內容
            var orderDetails = db.tOrderDetail.Where
                (m => m.fUserId == fUserId && m.fIsApproved == "否")
                .ToList();
            //View使用orderDetails模型
            return View(orderDetails);
        }


        //Get:Member/AddCar
        public ActionResult AddCar(string fPId)
        {
            //取得會員帳號並指定給fUserId
            fUserId = User.Identity.Name;
            var zproducts = db.tProduct
            .Where(m => m.fPId == fPId)
            .FirstOrDefault();

                zproducts.fstate = 2;
                //找出會員放入訂單明細的產品，該產品的fIsApproved為"否"
                //表示該產品是購物車狀態
                var currentCar = db.tOrderDetail
                        .Where(m => m.fcid == fPId && m.fIsApproved == "否" && m.fUserId == fUserId)
                        .FirstOrDefault();
                //若currentCar等於null，表示會員選購的產品不是購物車狀態
                if (currentCar == null)
                {
                    //找出目前選購的產品並指定給product
                    var product = db.tProduct.Where(m => m.fPId == fPId).FirstOrDefault();
                    //將產品放入訂單明細，因為產品的fIsApproved為"否"，表示為購物車狀態
                    tOrderDetail orderDetail = new tOrderDetail();
                    orderDetail.fUserId = fUserId;
                    orderDetail.flineID = product.fLine;
                    orderDetail.fPId = product.fPId;
                    orderDetail.fcid = product.fPId;
                    orderDetail.fName = product.fName;
                    orderDetail.fPrice = product.fPrice;
                    orderDetail.fQty = 1;
                    orderDetail.fIsApproved = "否";
                    db.tOrderDetail.Add(orderDetail);
                }
                //else
                //{
                //    //若產品為購物車狀態，跳出提示
                //}
            



            //db.SaveChanges();
            //return RedirectToAction("ShoppingCar");
 

            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }


        //Get:Member/DeleteCar
        public ActionResult DeleteCar(int fId, string fcid)
        {
            var zproducts = db.tProduct
           .Where(m => m.fPId == fcid)
           .FirstOrDefault();

            //if (zproducts != null) {
                zproducts.fstate = 1;
            //}

            // 依fId找出要刪除購物車狀態的產品
            var orderDetail = db.tOrderDetail.Where
                (m => m.fId == fId).FirstOrDefault();
            //刪除購物車狀態的產品
            db.tOrderDetail.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        //Post:Member/ShoppingCar
        [HttpPost]
        public ActionResult ShoppingCar(string fReceiver, string fEmail, string fAddress)
        {
            int CarCount;

            //找出會員帳號並指定給fUserId
            string fUserId = User.Identity.Name;
            //建立唯一的識別值並指定給guid變數，用來當做訂單編號
            //tOrder的fOrderGuid欄位會關聯到tOrderDetail的fOrderGuid欄位
            //形成一對多的關係，即一筆訂單資料會對應到多筆訂單明細
            string guid = Guid.NewGuid().ToString();
            //建立訂單主檔資料
            tOrder order = new tOrder();
            order.fOrderGuid = guid;
            order.fUserId = fUserId;
            order.fReceiver = fReceiver;
            order.fEmail = fEmail;
            order.fAddress = fAddress;
            order.fDate = DateTime.Now;
            db.tOrder.Add(order);
            //找出目前會員在訂單明細中是購物車狀態的產品
            var carList = db.tOrderDetail
                .Where(m => m.fIsApproved == "否" && m.fUserId == fUserId)
                .ToList();

            CarCount = carList.Count;

            if (CarCount == 0) {
                Response.Write("<script language='javascript'>alert('購物車無資料')");
                return RedirectToAction("ShoppingCar");
            }

            //將購物車狀態產品的fIsApproved設為"是"，表示確認訂購產品
            foreach (var item in carList)
            {
                item.fOrderGuid = guid;
                item.fIsApproved = "是";
            }
            //更新資料庫，異動tOrder和tOrderDetail
            //完成訂單主檔和訂單明細的更新
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }


        //Get:Member/OrderList
        public ActionResult OrderList()
        {
            //找出會員帳號並指定給fUserId
            string fUserId = User.Identity.Name;
            //找出目前會員的所有訂單主檔記錄並依照fDate進行遞增排序
            //將查詢結果指定給orders
            var orders = db.tOrder.Where(m => m.fUserId == fUserId)
                .OrderByDescending(m => m.fDate).ToList();
            //目前會員的訂單主檔OrderList.cshtml檢視使用orders模型
            return View(orders);
        }


        //Get:Member/OrderDetail
        public ActionResult OrderDetail(string fOrderGuid)
        {
            //根據fOrderGuid找出和訂單主檔關聯的訂單明細，並指定給orderDetails
            var orderDetails = db.tOrderDetail
                .Where(m => m.fOrderGuid == fOrderGuid).ToList();
            //目前訂單明細的OrderDetail.cshtml檢視使用orderDetails模型
            return View(orderDetails);
        }

        public class fdepartment
        {
            public static List<SelectListItem> EducationDropDownList =>
           new List<SelectListItem>()
           {
                new SelectListItem() {Text = "國際企業系所", Value = "國際企業系所"},
                new SelectListItem() {Text = "風險管理與保險學系/所", Value = "風險管理與保險學系/所"},
                new SelectListItem() {Text = "會計學系/所", Value = "會計學系/所"},
                new SelectListItem() {Text = "企業管理學系/所", Value = "企業管理學系/所"},
                new SelectListItem() {Text = "財務金融學系/所", Value = "財務金融學系/所"},
                new SelectListItem() {Text = "資訊管理學系/所", Value = "資訊管理學系/所"},
                new SelectListItem() {Text = "資訊傳播工程學系/所", Value = "資訊傳播工程學系/所"},
                new SelectListItem() {Text = "資訊工程學系/所", Value = "資訊工程學系/所"},
                new SelectListItem() {Text = "電腦與通訊工程學系/所", Value = "電腦與通訊工程學系/所"},
                new SelectListItem() {Text = "電子工程學系/所", Value = "電子工程學系/所"},
                new SelectListItem() {Text = "廣播電視學系", Value = "廣播電視學系"},
                new SelectListItem() {Text = "新聞學系", Value = "新聞學系"},
                new SelectListItem() {Text = "廣告暨策略行銷學系", Value = "廣告暨策略行銷學系"},
                new SelectListItem() {Text = "新媒體暨傳播管理學系/所", Value = "新媒體暨傳播管理學系/所"},
                new SelectListItem() {Text = "觀光事業學系 /所", Value = "觀光事業學系 /所"},
                new SelectListItem() {Text = "休閒遊憩管理學系", Value = "休閒遊憩管理學系"},
                new SelectListItem() {Text = "餐旅管理學系", Value = "餐旅管理學系"},
                new SelectListItem() {Text = "應用中國文學系/所", Value = "應用中國文學系/所"},
                new SelectListItem() {Text = "應用英語學系/所", Value = "應用英語學系/所"},
                new SelectListItem() {Text = "應用日語學系/所", Value = "應用日語學系/所"},
                new SelectListItem() {Text = "英語教學中心", Value = "英語教學中心"},
                new SelectListItem() {Text = "教育研究所", Value = "教育研究所"},
                new SelectListItem() {Text = "華語文教學學系", Value = "華語文教學學系"},
                new SelectListItem() {Text = "商業設計學系/所", Value = "商業設計學系/所"},
                new SelectListItem() {Text = "商品設計學系/所", Value = "商品設計學系/所"},
                new SelectListItem() {Text = "數位媒體設計學系/所", Value = "數位媒體設計學系/所"},
                new SelectListItem() {Text = "都市規劃與防災學系/所", Value = "都市規劃與防災學系/所"},
                new SelectListItem() {Text = "建築學系/所", Value = "建築學系/所"},
                new SelectListItem() {Text = "動漫文創設計學士學位學程", Value = "動漫文創設計學士學位學程"},
                new SelectListItem() {Text = "公共事務學系/所", Value = "公共事務學系/所"},
                new SelectListItem() {Text = "諮商與工商心理學系/所", Value = "諮商與工商心理學系/所"},
                new SelectListItem() {Text = "犯罪防治學系/所", Value = "犯罪防治學系/所"},
                new SelectListItem() {Text = "國際事務研究所", Value = "國際事務研究所"},
                new SelectListItem() {Text = "國際事務與外交學士學位學程", Value = "國際事務與外交學士學位學程"},
                new SelectListItem() {Text = "國際企業與貿易學士學位學程", Value = "國際企業與貿易學士學位學程"},
                new SelectListItem() {Text = "新聞與大眾傳播學士學位學程", Value = "新聞與大眾傳播學士學位學程"},
                new SelectListItem() {Text = "時尚與創新管理學士學位學程", Value = "時尚與創新管理學士學位學程"},
                new SelectListItem() {Text = "旅遊與觀光學士學位學程", Value = "旅遊與觀光學士學位學程"},
                new SelectListItem() {Text = "資訊科技應用與管理學士學位學程", Value = "資訊科技應用與管理學士學位學程"},
                new SelectListItem() {Text = "經濟與金融學系/所", Value = "經濟與金融學系/所"},
                new SelectListItem() {Text = "應用統計與資料科學學系/所", Value = "應用統計與資料科學學系/所"},
                new SelectListItem() {Text = "金融科技應用學士/碩士學位學程", Value = "金融科技應用學士/碩士學位學程"},
                new SelectListItem() {Text = "其他", Value = "其他"},
           };
        }
        public class fclass
        {
            public static List<SelectListItem> EducationDropDownList =>
           new List<SelectListItem>()
           {
               new SelectListItem(){Text="一",Value="一"},
               new SelectListItem(){Text="二",Value="二"},
               new SelectListItem(){Text="三",Value="三"},
               new SelectListItem(){Text="四",Value="四"},
           };
        }


    }
}