using NTTShopAdmin.Entities;
using NTTShopAdmin.Models;
using NTTShopAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList.Mvc;
using PagedList;
using NTTShopAdmin.Models.ViewModel;
using System.Xml.Linq;
using System.Reflection;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using System.Windows.Forms;
using System.Web.UI.WebControls;


namespace NTTShopAdmin.Controllers
{
    public class UsuariosController : Controller
    {
        ModelDAC model = new ModelDAC();
        private UsuariosViewModel modeloActual = new UsuariosViewModel();
        public ActionResult Usuarios(int? pageManagement, int? pageUser)
        {

            if (Session["session-id"] == null || string.IsNullOrWhiteSpace(Session["session-id"].ToString()))
            {
                // Redirigir a la acción de inicio de sesión
                return RedirectToAction("Login", "Login");
            }
            var viewModel = new UsuariosViewModel();
         
            int pageSizeManagement = 5;
            int pageSizeUser = 5;
            int pageNumberManagement = pageManagement ?? 1;
            int pageNumberUser = pageUser ?? 1;
            viewModel.languagesList = model.GetAllLanguage().ToList();
            viewModel.ManagementUsers = model.GetAllManagementUser().ToPagedList(pageNumberManagement, pageSizeManagement);
            viewModel.Users = model.GetAllUser().ToPagedList(pageNumberUser, pageSizeUser);
            Session["modeloActual"] = viewModel;
          

            return View(viewModel);
        }

        // GET: Ejemplo2/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ejemplo2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ejemplo2/Create
        [HttpPost]
        public ActionResult Create(System.Windows.Forms.FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Usuarios");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ejemplo2/Edit/5
        public ActionResult EditarAdmin(int id)
        {
            ManagementUser user = model.GetUserManagm(id);
            modeloActual = (UsuariosViewModel)Session["modeloActual"];
            modeloActual.UserId = user.PkUser.ToString();
            modeloActual.UserName = user.Name;
            modeloActual.Surname1 = user.Surname1;
            modeloActual.Surname2 = user.Surname2;
            modeloActual.UserPhone ="";
            modeloActual.UserEmail = user.Email;
            modeloActual.UserIdioma = user.Languages;

            return View("Usuarios", modeloActual);
         
        }

        public ActionResult EditarUsuario(int id)
        {
            User user = model.GetUser(id);
            modeloActual = (UsuariosViewModel)Session["modeloActual"];
            modeloActual.UserId = user.PkUser.ToString();
            modeloActual.UserName = user.Name;
            modeloActual.Surname1 = user.Surname1;
            modeloActual.Surname2 = user.Surname2;
            modeloActual.UserPhone = user.Phone;
            modeloActual.UserEmail = user.Email;
            modeloActual.UserIdioma = user.Languages;

            return View("Usuarios", modeloActual);

        }

        // POST: Ejemplo2/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, System.Windows.Forms.FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Usuarios");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ejemplo2/Delete/5
        public ActionResult btnBorrar_Click()
        {
            return View();
        }

        // POST: Ejemplo2/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, System.Windows.Forms.FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Usuarios");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Guardar(string action, string txtUser, string txtNombre, string txtApellido1, string txtApellido2, string txtPhone, string txtEmail, string txtIdioma)
        {
            modeloActual = (UsuariosViewModel)Session["modeloActual"];

            if (action == "Añadir")
            {
                return RedirectToAction("NewUser", "NewUser");
            }
            else
            {
                bool esUser = false;
                bool esAdmin = false;
                if (model.GetUser(int.Parse(txtUser)) != null || model.GetUserManagm(int.Parse(txtUser)) != null)
                {


                    if ((model.GetUser(int.Parse(txtUser)) != null && model.GetUserManagm(int.Parse(txtUser)) != null))
                    {
                        User user = model.GetUser(int.Parse(txtUser));
                        ManagementUser userAdmin = model.GetUserManagm(int.Parse(txtUser));
                        if (user.Email == txtEmail)
                        {
                            esUser = true;
                        }
                        else if (userAdmin.Email == txtEmail)
                        {
                            esAdmin = true;
                        }

                        if (esUser)
                        {
                            if (action == "Guardar")
                            {
                                string error = "";
                                User userActualizar = model.GetUser(int.Parse(txtUser));

                                userActualizar.Name = txtNombre;
                                userActualizar.Surname1 = txtApellido1;
                                userActualizar.Surname2 = txtApellido2;
                                userActualizar.Phone = txtPhone;
                                userActualizar.Email = txtEmail;
                                userActualizar.Languages = txtIdioma;
                                if (!model.UpdateUser(userActualizar, out error))
                                {
                                    ViewBag.ErrorMessage = $"Error: No se ha actualizado el usuario";
                                }
                                else
                                {
                                    MessageBox.Show("Se ha actualizado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }




                                return RedirectToAction("Usuarios", "Usuarios");
                            }
                            else if (action == "Eliminar")
                            {
                                modeloActual = (UsuariosViewModel)Session["modeloActual"];
                                User userActualizar = model.GetUser(int.Parse(txtUser));
                                List<Order> orders = model.GetAllOrder();


                                if(!orders.Any(order => order.idUser == userActualizar.PkUser))
                                {
                                    if (!model.DeleteUser(userActualizar.PkUser))
                                    {
                                        ViewBag.ErrorMessage = $"Error: No se ha podido eliminar el usuario";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Atención", "Se ha eliminado el usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Se ha eliminado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }

                                return RedirectToAction("Usuarios", "Usuarios");
                            }
                        }
                        else
                        {
                            if (action == "Guardar")
                            {
                                string error = "";
                                ManagementUser userAdminActualizar = model.GetUserManagm(int.Parse(txtUser));

                                userAdminActualizar.Name = txtNombre;
                                userAdminActualizar.Surname1 = txtApellido1;
                                userAdminActualizar.Surname2 = txtApellido2;
                                userAdminActualizar.Email = txtEmail;
                                user.Languages = txtIdioma;
                                if (!model.UpdateUserManag(userAdminActualizar, out error))
                                {
                                    ViewBag.ErrorMessage = $"Error: No se ha actualizado el usuario";
                                    MessageBox.Show("Atención", "Error: \n no se ha actualizado el usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                                else
                                {
                                    MessageBox.Show("Se ha actualizado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                }



                                return RedirectToAction("Usuarios", "Usuarios");
                            }
                            else if (action == "Eliminar")
                            {
                                modeloActual = (UsuariosViewModel)Session["modeloActual"];
                                ManagementUser userAdminAztualizar = model.GetUserManagm(int.Parse(txtUser));

                                if (!model.DeleteUserManag(userAdminAztualizar.PkUser))
                                {
                                    ViewBag.ErrorMessage = $"Error: No se ha podido eliminar el usuario";
                                }
                                else
                                {
                                    MessageBox.Show("Se ha eliminado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                }
                                return RedirectToAction("Usuarios", "Usuarios");
                            }
                        }
                    }
                    else if (model.GetUser(int.Parse(txtUser)) != null)
                    {
                        if (action == "Guardar")
                        {
                            string error = "";
                            User user = model.GetUser(int.Parse(txtUser));

                            user.Name = txtNombre;
                            user.Surname1 = txtApellido1;
                            user.Surname2 = txtApellido2;
                            user.Phone = txtPhone;
                            user.Email = txtEmail;
                            user.Languages = txtIdioma;
                            if (!model.UpdateUser(user, out error))
                            {
                                ViewBag.ErrorMessage = $"Error: No se ha actualizado el usuario";

                            }
                            else
                            {
                                MessageBox.Show("Se ha actualizado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            }



                            return RedirectToAction("Usuarios", "Usuarios");
                        }
                        else if (action == "Eliminar")
                        {
                            modeloActual = (UsuariosViewModel)Session["modeloActual"];
                            User user = model.GetUser(int.Parse(txtUser));

                            if (!model.DeleteUser(user.PkUser))
                            {
                                ViewBag.ErrorMessage = $"Error: No se ha podido eliminar el usuario";
                            }
                            else
                            {
                                MessageBox.Show("Se ha eliminado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return RedirectToAction("Usuarios", "Usuarios");
                        }

                    }
                    else
                    {
                        if (action == "Guardar")
                        {
                            string error = "";
                            ManagementUser userActualizar = model.GetUserManagm(int.Parse(txtUser));

                            userActualizar.Name = txtNombre;
                            userActualizar.Surname1 = txtApellido1;
                            userActualizar.Surname2 = txtApellido2;
                            userActualizar.Email = txtEmail;
                            userActualizar.Languages = txtIdioma;
                            if (!model.UpdateUserManag(userActualizar, out error))
                            {
                                ViewBag.ErrorMessage = $"Error: No se ha actualizado el usuario";
                            }
                            else
                            {
                                MessageBox.Show("Se ha actualizado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            }



                            return RedirectToAction("Usuarios", "Usuarios");
                        }
                        else if (action == "Eliminar")
                        {
                            modeloActual = (UsuariosViewModel)Session["modeloActual"];
                            ManagementUser userActualizar = model.GetUserManagm(int.Parse(txtUser));

                            if (!model.DeleteUserManag(userActualizar.PkUser))
                            {
                                ViewBag.ErrorMessage = $"Error: No se ha podido eliminar el usuario";
                            }
                            else
                            {
                                MessageBox.Show("Se ha eliminado el usuario", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            return RedirectToAction("Usuarios", "Usuarios");
                        }
                    }
                }
            }
           

               return RedirectToAction("Usuarios", "Usuarios");
        }


    }
}
