
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using prjShoppingCar.Models;
using PagedList;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace prjShoppingCar.Controllers
{[Authorize]
    public class HomeController : Controller
    {
        //建立可存取dbShoppingCar.mdf 資料庫的dbShoppingCarEntities 類別物件db
        dbShoppingCarEntities db = new dbShoppingCarEntities();
        int pageSize = 3;
        // GET: Home/Index 
        public ActionResult Index(string zffaculty, string zfdepartment, int page = 1)
        {
            //取得所有產品放入products
            int currentPage = page < 1 ? 1 : page;
            var products = db.tProduct
                .OrderByDescending(m => m.fId).ToList();

            //對產品列表做篩選 符合分類的才顯示
            //zCategory = zCategory;
            if (zffaculty != null)
            {
                foreach (var zproducts in products.ToArray())
                {
                    if (zproducts.ffaculty != zffaculty) { products.Remove(zproducts); }
                }
            }
            var result = products.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        [AllowAnonymous]
     
        public ActionResult Login()
        {
            return View();
        }
        //Post: Home/Login
        [AllowAnonymous]

        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {
            // 依帳密取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == fUserId && m.fPwd == fPwd)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            //使用Session變數記錄歡迎詞
            Session["WelCome"] = member.fName + "歡迎光臨";
            FormsAuthentication.RedirectFromLoginPage(fUserId, true);
            return RedirectToAction("Index", "Member");
        }
        [AllowAnonymous]
        //Get:Home/Register
        public ActionResult Register()
        {
            return View();
        }
        //Post:Home/Register
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(tMember pMember)
        {
            //若模型沒有通過驗證則顯示目前的View
            if (ModelState.IsValid == false)
            {
                return View();
            }
            // 依帳號取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == pMember.fUserId)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                //將會員記錄新增到tMember資料表
                db.tMember.Add(pMember);
                db.SaveChanges();
                //執行Home控制器的Login動作方法
                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號己有人使用，註冊失敗";
            return View();
        }
    }



    public class ffaculty
    {
        public static List<SelectListItem> EducationDropDownList =>
       new List<SelectListItem>()
       {
                new SelectListItem() {Text = "管理學院", Value = "管理學院"},
                new SelectListItem() {Text = "資訊學院", Value = "資訊學院"},
       };
    }

    public class fdepartment
    {
        public static List<SelectListItem> EducationDropDownList =>
       new List<SelectListItem>()
       {
                new SelectListItem() {Text = "國際企業系所", Value = "國際企業系所"},
                new SelectListItem() {Text = "風險管理與保險學系/所", Value = "風險管理與保險學系/所"},
                new SelectListItem() {Text = "資訊管理學系/所", Value = "資訊管理學系/所"},
                new SelectListItem() {Text = "資訊傳播工程學系/所", Value = "資訊傳播工程學系/所"},
       };
    }
}