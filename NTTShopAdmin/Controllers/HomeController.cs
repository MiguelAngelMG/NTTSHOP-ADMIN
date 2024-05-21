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
            if (Session["session-id"] == null || string.IsNullOrWhiteSpace(Session["session-id"].ToString()))
            {
                // Redirigir a la acción de inicio de sesión
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}