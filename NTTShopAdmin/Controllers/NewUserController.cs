using NTTShopAdmin.Entities;
using NTTShopAdmin.Models;
using NTTShopAdmin.Models.Entities;
using NTTShopAdmin.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Forms;

namespace NTTShopAdmin.Controllers
{
    public class NewUserController : Controller
    {
        ModelDAC model = new ModelDAC();
        private NewUserViewModel modeloActual;
        public ActionResult NewUser()
        {
            if (Session["session-id"] == null || string.IsNullOrWhiteSpace(Session["session-id"].ToString()))
            {
                // Redirigir a la acción de inicio de sesión
                return RedirectToAction("Login", "Login");
            }

            var viewModel = new NewUserViewModel();

            viewModel.languagesList = model.GetAllLanguage().ToList();
            Session["modeloActual"] = viewModel;


            return View(viewModel);

            
        }

        [HttpPost]
        public ActionResult Guardar(string action, string txtLanguage, string txtEmail, string txtApellido2, string txtApellido1, string txtPassword, string txtLogin, string txtNombre)
        {
            if (action == "Cancelar")
            {

                return RedirectToAction("Usuarios", "Usuarios");
            }
            modeloActual = (NewUserViewModel)Session["modeloActual"];

            if (ValidarCampos(action, txtLanguage, txtEmail,txtApellido2, txtApellido1, txtPassword, txtLogin, txtNombre))
            {
                ManagementUser user = new ManagementUser()
                {
                    login = txtLogin,
                    Password = txtPassword,
                    Name = txtNombre,
                    Surname1 = txtApellido1,
                    Surname2 = txtApellido2,
                    Email = txtEmail,
                    Languages = txtLanguage,
                };

                if (ValidationPassword(txtPassword))
                {
                    if (action == "Añadir")
                    {
                        


                        if (!model.InsertManagementUser(user))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha introducido el producto";
                            modeloActual.user = user;
                        }
                        else
                        {
                            MessageBox.Show("Se ha añadido el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return RedirectToAction("Usuarios", "usuarios");
                        }


                    }
                    
                }
                else
                {
                    ViewBag.ErrorMessage = $"Error: Contraseña no valida";
                    user.Password = "";
                    modeloActual.user = user;
                }
            }
            else 
            {
                ViewBag.ErrorMessage = $"Error: No se ha insertado el usuario";

            }

            return View("NewUser", modeloActual);
        }
        public bool ValidarCampos(string action, string txtLanguage, string txtEmail, string txtApellido2, string txtApellido1, string txtPassword, string txtLogin, string txtNombre)
        {
            
            if (string.IsNullOrEmpty(action) ||
                string.IsNullOrEmpty(txtLanguage) ||
                string.IsNullOrEmpty(txtNombre) ||
                string.IsNullOrEmpty(txtApellido1) ||
                string.IsNullOrEmpty(txtApellido2) ||
                string.IsNullOrEmpty(txtEmail) ||
                string.IsNullOrEmpty(txtLogin) ||
                string.IsNullOrEmpty(txtPassword))

            {
                return false;
            }
            return true;
        }
        private bool ValidationPassword(string password)
        {
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{1,}$"; //Debe tener minimo una mayuscula, minuscula y numero 

            if (Regex.IsMatch(password, regex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
