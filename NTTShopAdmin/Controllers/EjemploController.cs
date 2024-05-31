﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTTShopAdmin.Controllers
{
    public class EjemploController : Controller
    {
        // GET: Ejemplo
        public ActionResult Ejemplo()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
