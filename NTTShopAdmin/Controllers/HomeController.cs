using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTTShopAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult About()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}