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

namespace NTTShopAdmin.Controllers
{
    public class NewProductController : Controller
    {
        ModelDAC model = new ModelDAC();
        private NewUserViewModel modeloActual;
        public ActionResult NewProduct()
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
        public ActionResult Guardar(string action, string txtLanguage, string txtStock, string txtDisponible, string txtTitle, string txtDescripcion, string txtPrecio)
        {
            modeloActual = (NewUserViewModel)Session["modeloActual"];

            if (ValidarCampos(action, txtLanguage, txtStock,txtDisponible, txtTitle, txtDescripcion, txtPrecio))
            {
                if (action == "Añadir")
                {
                    Product product = new Product();    
                    ProductDescription description = new ProductDescription();
                    ProductRates rate = new ProductRates();

                    product.stock = int.Parse(txtStock);
                    product.enabled = bool.Parse(txtDisponible);

                    description.title = txtTitle;
                    description.description = txtDescripcion;
                    description.language = txtLanguage;
                    product.description.Add(description);
                    rate.idRate = 1;
                    rate.price = decimal.Parse(txtPrecio);
                    product.rates.Add(rate);

                    if (!model.InsertProduct(product,out int IdCreado))
                    {
                        ViewBag.ErrorMessage = $"Error: No se ha introducido el producto";
                        
                    }
                    rate.idProduct = IdCreado;
                    if (!model.UpdateProductRate(rate))
                    {
                        ViewBag.ErrorMessage += "Error: No se ha introducido pecrio del producto";
                    }


                    return RedirectToAction("Productos", "Productos");
                }
                else if (action == "Cancelar")
                {

                    return RedirectToAction("Productos", "Productos");
                }
            }
            else 
            {
                ViewBag.ErrorMessage = $"Error: No se ha insertado el Language";

            }
            return RedirectToAction("Productos", "Productos");
        }
        [HttpPost]
        public ActionResult GuardarDescription(string action, string txtIdDescription, string txtIdProduct, string txtLanguage, string txtTitle, string txtDescription)
        {
        //    modeloActual = (ProductViewModel)Session["modeloActual"];

            if (!string.IsNullOrEmpty(txtIdProduct) && model.GetProduct(int.Parse(txtIdProduct)) != null)
            {
                if (action == "Guardar")
                {
                    string error = "";
                    Product product = model.GetProduct(int.Parse(txtIdProduct));
                    ProductDescription description = new ProductDescription();
                    description.idProductDescription = int.Parse(txtIdDescription);
                    description.idProduct = int.Parse(txtIdProduct);
                    description.language = txtLanguage;
                    description.title = txtTitle;
                    description.description = txtDescription;
                    int pos = 0;
                    int i = 0;
                    foreach (var des in product.description)
                    {

                        if (des.idProductDescription == int.Parse(txtIdDescription))
                        {
                            pos = i;
                        }
                        i++;
                    }
                    product.description[pos] = description;
                    if (!model.UpdateProduct(product))
                    {
                        ViewBag.ErrorMessage = $"Error: No se ha actualizado la descripción";
                    }



                    return RedirectToAction("Productos", "Productos");
                }
                else if (action == "Eliminar")
                {
                    //modeloActual = (ProductViewModel)Session["modeloActual"];
                    //Product product = model.GetProduct(int.Parse(txtIdProduct));
                    //int idDescription = int.Parse(txtIdDescription);
                    //product.description.RemoveAll(des => des.idProductDescription == idDescription);
                    //foreach (var pro in modeloActual.productsList)
                    //{
                    //    if (pro.idProduct == product.idProduct)
                    //    {
                    //        pro.description.RemoveAll(des => des.idProductDescription == idDescription);
                    //    }
                    //}
                    //if (!model.DeleteProductDescription(product.idProduct, idDescription))
                    //{
                    //    ViewBag.ErrorMessage = $"Error: No se ha eliminado la descripción";
                    //}

                    return RedirectToAction("Productos", "Productos");
                }
                else if (action == "Añadir")
                {
                    Product product = model.GetProduct(int.Parse(txtIdProduct));
                    ProductDescription description = new ProductDescription();


                    description.idProduct = int.Parse(txtIdProduct);
                    description.language = txtLanguage;
                    description.title = txtTitle;
                    description.description = txtDescription;

                    product.description.Add(description);
                    if (!model.UpdateProduct(product))
                    {
                        ViewBag.ErrorMessage = $"Error: No se ha añadido el producto";
                    }



                    return RedirectToAction("Productos", "Productos");
                }


            }
            else if (action == "Añadir")
            {
                Product product = model.GetProduct(int.Parse(txtIdProduct));
                ProductDescription description = new ProductDescription();


                description.idProduct = int.Parse(txtIdProduct);
                description.language = txtLanguage;
                description.title = txtTitle;
                description.description = txtDescription;

                product.description.Add(description);
                if (!model.UpdateProduct(product))
                {
                    ViewBag.ErrorMessage = $"Error: No se ha añadido el producto";
                }



                return RedirectToAction("Productos", "Productos");
            }
            return RedirectToAction("Productos", "Productos");
        }

        public bool ValidarCampos(string action, string txtLanguage, string txtStock, string txtDisponible, string txtTitle, string txtDescripcion, string txtPrecio)
        {
            
            if (string.IsNullOrEmpty(action) ||
                string.IsNullOrEmpty(txtLanguage) ||
                string.IsNullOrEmpty(txtStock) ||
                string.IsNullOrEmpty(txtDisponible) ||
                string.IsNullOrEmpty(txtTitle) ||
                string.IsNullOrEmpty(txtDescripcion) ||
                string.IsNullOrEmpty(txtPrecio))
            {
                return false;
            }
            return true;
        }
    }

}
