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
    public class ProductosController : Controller
    {
        ModelDAC model = new ModelDAC();
        private ProductViewModel modeloActual;
        public ActionResult Productos(int? pageProduct, int? pageDescription, int? pageRates)
        {
            if (Session["session-id"] == null || string.IsNullOrWhiteSpace(Session["session-id"].ToString()))
            {
                // Redirigir a la acción de inicio de sesión
                return RedirectToAction("Login", "Login");
            }
            if (Session["modeloActual"] == null || !(Session["modeloActual"] is ProductViewModel))
            {
            var viewModel = new ProductViewModel();

            int pageSizeProduct = 5;
            int pageSizeDescription = 5;
            int pageSizeRate = 5;
            int pageNumberProduct = pageProduct ?? 1;
            int pageNumberDescription = pageDescription ?? 1;
            int pageNumberRate = pageRates ?? 1;
            viewModel.allProducts = model.GetAllProducts(-1);
            viewModel.productsList = viewModel.allProducts.ToPagedList(pageNumberProduct, pageSizeProduct);
            viewModel.descriptionsList = new List<ProductDescription>().ToPagedList(pageNumberDescription, pageSizeDescription);
            viewModel.ratesList = new List<ProductRates>().ToPagedList(pageNumberRate, pageSizeRate);
            viewModel.languagesList = model.GetAllLanguage().ToList();
            viewModel.allRates = model.GetAllRate().ToList();
            Session["modeloActual"] = viewModel;


            return View(viewModel);

            }
            else
            {
                modeloActual = (ProductViewModel)Session["modeloActual"];
                int pageSizeProduct = 5;
                int pageSizeDescription = 5;
                int pageSizeRate = 5;
                int pageNumberProduct = pageProduct ?? 1;
                int pageNumberDescription = pageDescription ?? 1;
                int pageNumberRate = pageRates ?? 1;


                modeloActual.productsList = modeloActual.allProducts.ToPagedList(pageNumberProduct, pageSizeProduct);
                modeloActual.descriptionsList = modeloActual.allProductsDescription.ToPagedList(pageNumberDescription, pageSizeDescription);
                modeloActual.ratesList = modeloActual.allProductsRate.ToPagedList(pageNumberRate, pageSizeRate);


                return View("Productos", modeloActual);
            }
            
        }
        
        [HttpPost]
        public ActionResult GuardarProducto(string action, string txtIdProduct, string txtStock, string txtDisponible, string txtTitle, string txtDescripcion, string txtPrecio)
        {
            modeloActual = (ProductViewModel)Session["modeloActual"];
            if (modeloActual.product != null && modeloActual.product.idProduct != 0)
            {
                if (!string.IsNullOrEmpty(txtIdProduct) && model.GetProduct(int.Parse(txtIdProduct)) != null)
                {
                    if (action == "Guardar")
                    {
                        string error = "";
                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        product.stock = int.Parse(txtStock);
                        product.enabled = bool.Parse(txtDisponible);

                        if (!model.UpdateProduct(product))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha actualizado la descripción";
                        }
                        else
                        {
                            MessageBox.Show("Se ha actualizado el producto", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            modeloActual.productsList = model.GetAllProducts(-1).ToPagedList(1, 5);
                        }

                        Session["modeloActual"] = modeloActual;

                        return View("Productos", modeloActual);

                    }
                    else if (action == "Eliminar")
                    {
                        modeloActual = (ProductViewModel)Session["modeloActual"];

                        if (!model.DeleteProduct(int.Parse(txtIdProduct)))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha podido eliminar el Language";
                        }
                        else
                        {
                            MessageBox.Show("Se ha eliminado el producto", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        return RedirectToAction("Productos", "Productos");
                    }
                    else if (action == "Añadir")
                    {

                        return RedirectToAction("NewProduct", "NewProduct");
                    }


                }
                else if (action == "Añadir")
                {

                    return RedirectToAction("NewProduct", "NewProduct");

                }
            }
            else
            {
                ViewBag.ErrorMessageProduct = $"Error: Tienes que seleccionar un producto";
            }
            if (action == "Añadir")
            {
                return RedirectToAction("NewProduct", "NewProduct");
            }

            return View("Productos", modeloActual);

        }
        [HttpPost]
        public ActionResult GuardarDescription(string action, string txtIdDescription, string txtIdProduct, string txtLanguage, string txtTitle, string txtDescription)
        {
            modeloActual = (ProductViewModel)Session["modeloActual"];

            if (modeloActual.productDescription != null)
            {
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
                        modeloActual = (ProductViewModel)Session["modeloActual"];
                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        int idDescription = int.Parse(txtIdDescription);
                        product.description.RemoveAll(des => des.idProductDescription == idDescription);
                        foreach (var pro in modeloActual.productsList)
                        {
                            if (pro.idProduct == product.idProduct)
                            {
                                pro.description.RemoveAll(des => des.idProductDescription == idDescription);
                            }
                        }
                        if (!model.DeleteProductDescription(product.idProduct, idDescription))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha eliminado la descripción";
                        }

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
            }
            else
            {
                ViewBag.ErrorMessageDescription = $"Error: Debes seleccionar una descripción ";
            }
            return RedirectToAction("Productos", "Productos");
        }
        [HttpPost]
        public ActionResult GuardarRate(string action, string txtIdRate, string txtIdProduct, string txtPrecie)
            {
                modeloActual = (ProductViewModel)Session["modeloActual"];
            if(modeloActual.productRates != null)
            {
                if (!string.IsNullOrEmpty(txtIdProduct) && model.GetProduct(int.Parse(txtIdProduct)) != null)
                {
                    if (action == "Guardar")
                    {
                        string error = "";
                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        ProductRates rate = new ProductRates();
                        rate.idRate = int.Parse(txtIdRate);
                        rate.idProduct = int.Parse(txtIdProduct);
                        rate.price = int.Parse(txtPrecie);


                        int pos = 0;
                        int i = 0;
                        foreach (var des in product.rates)
                        {

                            if (des.idRate == int.Parse(txtIdRate))
                            {
                                pos = i;
                            }
                            i++;
                        }
                        product.rates[pos] = rate;
                        if (!model.UpdateProduct(product) || !model.UpdateProductRate(rate))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha actualizado la tarifa";
                        }
                        int j = 0;

                        foreach (var pro in modeloActual.productsList)
                        {
                            if (pro.idProduct == product.idProduct)
                            {
                                pro.rates[pos] = rate;
                            }
                            j++;
                        }

                        Session["modeloActual"] = modeloActual;


                        return RedirectToAction("Productos", "Productos");
                    }
                    else if (action == "Eliminar")
                    {

                        modeloActual = (ProductViewModel)Session["modeloActual"];
                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        int idRate = int.Parse(txtIdRate);
                        product.rates.RemoveAll(des => des.idRate == idRate);
                        foreach (var pro in modeloActual.productsList)
                        {
                            if (pro.idProduct == product.idProduct)
                            {
                                pro.rates.RemoveAll(des => des.idRate == idRate);
                            }
                        }
                        if (!model.DeleteProductRate(product.idProduct, idRate))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha eliminado el rate";
                        }
                        Session["modeloActual"] = modeloActual;
                        return RedirectToAction("Productos", "Productos");
                    }
                    else if (action == "Añadir")
                    {

                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        ProductRates rate = new ProductRates();


                        rate.idRate = int.Parse(txtIdRate);
                        rate.idProduct = int.Parse(txtIdProduct);
                        rate.price = int.Parse(txtPrecie);

                        int i = 0;
                        product.rates.Add(rate);

                        if (!model.UpdateProduct(product) && !model.UpdateProductRate(rate))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha añadido el producto";

                        }
                        else
                        {
                            foreach (var pro in modeloActual.productsList)
                            {
                                if (pro.idProduct == product.idProduct)
                                {
                                    modeloActual.productsList[i].rates.Add(rate);
                                }
                                i++;
                            }
                            Session["modeloActual"] = modeloActual;

                        }


                        return RedirectToAction("Productos", "Productos");
                    }



                    else if (action == "Añadir")
                    {
                        Product product = model.GetProduct(int.Parse(txtIdProduct));
                        ProductRates rate = new ProductRates();


                        rate.idRate = int.Parse(txtIdRate);
                        rate.idProduct = int.Parse(txtIdProduct);
                        rate.price = int.Parse(txtPrecie);

                        int i = 0;
                        product.rates.Add(rate);

                        if (!model.UpdateProduct(product) && !model.UpdateProductRate(rate))
                        {
                            ViewBag.ErrorMessage = $"Error: No se ha añadido el producto";

                            foreach (var pro in modeloActual.productsList)
                            {
                                if (pro.idProduct == product.idProduct)
                                {
                                    modeloActual.productsList[i].rates.Add(rate);
                                }
                                i++;
                            }
                            Session["modeloActual"] = modeloActual;

                        }
                        return RedirectToAction("Productos", "Productos");
                    }

                    return RedirectToAction("Productos", "Productos");
                }
            }
            else
            {
                ViewBag.ErrorMessageRate = $"Error: Debes seleccionar un Rate ";
            }
            
            return RedirectToAction("Productos", "Productos");

        }

        public ActionResult EditarProductos(int id)
        {
            Product product = model.GetProduct(id);
            modeloActual = (ProductViewModel)Session["modeloActual"];
            modeloActual.product = product;
            modeloActual.product.stock = product.stock;

            modeloActual.allProductsDescription = product.description;
            modeloActual.allProductsRate = product.rates;
            modeloActual.descriptionsList = modeloActual.allProductsDescription.ToPagedList(1, 5);
            modeloActual.ratesList = modeloActual.allProductsRate.ToPagedList(1, 5);
            modeloActual.productDescription = product.description[0];
            modeloActual.productRates = product.rates[0];
            Session["modeloActual"] = modeloActual;

            return View("Productos", modeloActual);

        } 
        public ActionResult EditarDescripciones(int id, int description)
        {
            Product product = model.GetProduct(id);

            modeloActual = (ProductViewModel)Session["modeloActual"];
            foreach( var des in product.description)
            {
                if(des.idProductDescription == description)
                {
                    modeloActual.productDescription = des;
                }
            }


            return View("Productos", modeloActual);

        } 
        public ActionResult EditarRate(int id, int rate)
        {
            Product product = model.GetProduct(id);
            modeloActual = (ProductViewModel)Session["modeloActual"];
            foreach (var des in product.rates)
            {
                if (des.idRate == rate)
                {
                    modeloActual.productRates = des;
                }
            }

            return View("Productos", modeloActual);

        }
    }

}
