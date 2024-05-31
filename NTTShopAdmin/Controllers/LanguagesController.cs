using NTTShopAdmin.Entities;
using NTTShopAdmin.Models;
using NTTShopAdmin.Models.Entities;
using NTTShopAdmin.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Forms;

namespace NTTShopAdmin.Controllers
{
    public class LanguagesController : Controller
    {
        ModelDAC model = new ModelDAC();
        private LanguageViewModel modeloActual;
        public ActionResult Languages(int? pageLanguage)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var viewModel = new LanguageViewModel();

            int pageSizeLanguage = 5;
            int pageNumberLanguage = pageLanguage ?? 1;

            viewModel.Language = model.GetAllLanguage().ToPagedList(pageNumberLanguage, pageSizeLanguage);
           
            Session["modeloActual"] = viewModel;


            return View(viewModel);
        }

        // GET: Ejemplo2/Details/5
        public ActionResult Details(int id)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // GET: Ejemplo2/Create
        public ActionResult Create()
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: Ejemplo2/Create
        [HttpPost]
        public ActionResult Create(System.Windows.Forms.FormCollection collection)
        {
            try
            {
                if (Session["session-id"] == null && Session["session-language"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                return RedirectToAction("Usuarios");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ejemplo2/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: Ejemplo2/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, System.Windows.Forms.FormCollection collection)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
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
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: Ejemplo2/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, System.Windows.Forms.FormCollection collection)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Languages");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditarLanguage(int id)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            Language language = model.GetLanguage(id);
            modeloActual = (LanguageViewModel)Session["modeloActual"];
            modeloActual.idLanguage = language.idLanguage.ToString();
            modeloActual.descripcion = language.descripcion;
            modeloActual.iso = language.iso; 

            return View("Languages", modeloActual);

        }
        [HttpPost]
        public ActionResult Guardar(string action, string txtIdLanguage, string txtDescripcion, string txtIso)
        {
            if (Session["session-id"] == null && Session["session-language"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            modeloActual = (LanguageViewModel)Session["modeloActual"];

            if (!string.IsNullOrEmpty(txtIdLanguage) && model.GetLanguage(int.Parse(txtIdLanguage)) != null)
            {
                if (action == "Guardar")
                {
                    string error = "";
                    Language language = model.GetLanguage(int.Parse(txtIdLanguage));

                    language.idLanguage = int.Parse(txtIdLanguage);
                    language.descripcion = txtDescripcion;
                    language.iso = txtIso;
                    if (!model.UpdateLanguage(language, out error))
                    {
                        ViewBag.ErrorMessage = $"Error: No se ha actualizado el Language";
                    }
                    else
                    {
                        MessageBox.Show("Se ha actualizado el lenguaje", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }


                    return RedirectToAction("Languages", "Languages");
                }
                else if (action == "Eliminar")
                {
                    modeloActual = (LanguageViewModel)Session["modeloActual"];
                    Language language = model.GetLanguage(int.Parse(txtIdLanguage));

                    if (!model.DeleteLanguage(language.idLanguage))
                    {
                        MessageBox.Show("Error: No se puede eliminar el idioma, porque está en uso.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Se ha eliminado el lenguaje", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    return RedirectToAction("Languages", "Languages");
                }
                else if (action == "Añadir")
                {
                  
                    Language language = new Language();

                    
                    language.descripcion = txtDescripcion;
                    language.iso = txtIso;

                    if (!model.InsertLanguage(language))
                    {
                        ViewBag.ErrorMessage = $"Error: No se ha insertado el Language";
                    }



                    return RedirectToAction("Languages", "Languages");
                }


            }
            else if (action == "Añadir")
            {
                Language language = new Language();


                language.descripcion = txtDescripcion;
                language.iso = txtIso;

                if (!model.InsertLanguage(language))
                {
                    ViewBag.ErrorMessage = $"Error: No se ha insertado el Language";
                }



                return RedirectToAction("Languages", "Languages");

            }

            return RedirectToAction("Languages", "Languages");
        }
    }
}
