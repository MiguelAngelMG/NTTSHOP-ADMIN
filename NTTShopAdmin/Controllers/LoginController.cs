using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using NTTShopAdmin.Entities;
using NTTShopAdmin.Models;
using NTTShopAdmin.Models.Entities;


namespace NTTShopAdmin.Controllers
{

    public class LoginController : Controller
    {
        ModelDAC model = new ModelDAC();
        // GET: Login
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Login()
        {
            Session["session-id"] = null;
            Session["session-language"] = null;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ManagementUser objUser)
        {
            ManagementUser admin = new ManagementUser();

           
            if (ModelState.IsValid)
            {
                string usuario = objUser.login;
                string contraseña = objUser.Password;

                bool login = LoginUser(usuario, contraseña);

                if (login)
                {

                    return View("~/Views/Home/Index.cshtml");

                }
                else
                {

                }

            }
            
            return View(objUser);
        }


        public ActionResult UserDashBoard()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public bool LoginUser(string user, string password)
        {

            bool existeUsuario = false;

            try
            {

                string url = "https://localhost:7077/api/ManagementLogin/getLogin/" + user + "/" + password;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    existeUsuario = true;

                    int idUser;
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var json = JObject.Parse(result);
                        idUser = json["idUser"].ToObject<int>();

                    }
                    ManagementUser userlogin = model.GetUserManagm(idUser);
                    Session["session-id"] = idUser;
                    Session["session-language"] = userlogin.Languages;
                }
                else
                {
                    ViewBag.ErrorMessge = $"Error: Usuario o Password incorrecto!";
                    
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"No existe usuario con esas credenciales";
                //Console.WriteLine($"Error al hacer la solicitud: {ex.Message}");
                //string script = "alert(\"Error: Usuario o Password incorrecto! \");";
                //ScriptManager.RegisterStartupScript(this, GetType(),
                //                      "ServerControlScript", script, true);
            }

            return existeUsuario;
        }
        public ActionResult CerrarSesion()
        {
         return RedirectToAction("Login", "Login");
           
        }
    }
}
