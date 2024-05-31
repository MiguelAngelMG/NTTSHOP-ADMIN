using Antlr.Runtime.Misc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NTTShopAdmin.Entities;
using NTTShopAdmin.Models;
using NTTShopAdmin.Models.Entities;
using NTTShopAdmin.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Forms;

namespace NTTShopAdmin.Controllers
{
    public class OrdersController : Controller
    {
        ModelDAC model = new ModelDAC();
        private OrdersViewModel modeloActual;
        public ActionResult Orders(int? pageOrder, int? pageDetail)
        {
            if (Session["session-id"] == null || string.IsNullOrWhiteSpace(Session["session-id"].ToString()))
            {
                // Redirigir a la acción de inicio de sesión
                return RedirectToAction("Login", "Login");
            }
            if (Session["modeloActual"] == null || !(Session["modeloActual"] is OrdersViewModel))
            {
            var viewModel = new OrdersViewModel();

            int pageSizeProduct = 5;
            int pageSizeDescription = 10;
            int pageNumberProduct = pageOrder ?? 1;
            int pageNumberDescription = pageDetail ?? 1;
           
            viewModel.allOrders = model.GetAllOrder();
            viewModel.orderPaged = viewModel.allOrders.ToPagedList(pageNumberProduct, pageSizeProduct);
            viewModel.orderDetailPaged = new List<OrderDetail>().ToPagedList(pageNumberDescription, pageSizeDescription);
            viewModel.allOrderStatus = model.GetAllStatus();
            viewModel.searchOrderStatus.Add(new OrderStatus() { idStatus = -1, description = "" });
            viewModel.searchOrderStatus.AddRange(viewModel.allOrderStatus);
              
            Session["modeloActual"] = viewModel;


            return View(viewModel);

            }
            else
            {
                modeloActual = (OrdersViewModel)Session["modeloActual"];
                int pageSizeProduct = 5;
                int pageSizeDescription = 5;
                int pageNumberProduct = pageOrder ?? 1;
                int pageNumberDescription = pageDetail ?? 1;

                modeloActual.orderPaged = modeloActual.allOrders.ToPagedList(pageNumberProduct, pageSizeProduct);
                modeloActual.orderDetailPaged = modeloActual.allOrdersDetail.ToPagedList(pageNumberDescription, pageSizeDescription);


                return View("Orders", modeloActual);
            }
            
        }
        
        [HttpPost]
        public ActionResult Buscar(string action, string txtnombre, DateTime? desdeDateInput, DateTime? hastaDateinput, int txtEstado)
        {
            if (action == "Buscar")
            {

                modeloActual = (OrdersViewModel)Session["modeloActual"];
                string desdeDateFormatted = "";
                string hastaDateFormatted = "";
                
                if (desdeDateInput != null)
                {
                     DateTime desdeDate = desdeDateInput.Value;
                     desdeDateFormatted = $"{desdeDate.Year}/{desdeDate.Month}/{desdeDate.Day}";
                }
                if(hastaDateinput != null)
                {
                    DateTime hastaDate = hastaDateinput.Value;
                     hastaDateFormatted = $"{hastaDate.Year}/{hastaDate.Month}/{hastaDate.Day}";
                }
                
                

                List<Order> orderList = model.GetAllOrder();
                List<Order> listaDef = model.GetAllOrder(txtEstado, desdeDateFormatted, hastaDateFormatted);

                if (!string.IsNullOrEmpty(txtnombre))
                {
                    listaDef.RemoveAll(order => !order.userName().ToLower().Contains(txtnombre.ToLower()));
                }

                modeloActual.allOrders = listaDef;
                modeloActual.orderPaged = modeloActual.allOrders.ToPagedList(1, 5);
            }
            else if (action == "Resetear")
            {
                modeloActual = (OrdersViewModel)Session["modeloActual"];
                modeloActual.allOrders = model.GetAllOrder();
                modeloActual.orderPaged = modeloActual.allOrders.ToPagedList(1, 5);
            }

            Session["modeloActual"] = modeloActual;

            return View("Orders", modeloActual);
        }

        [HttpPost]
        public ActionResult GuardarOrder(string action, string txtIdOrder, string txtDateOrder, string txtTitle, string txtDescription, string txtLanguage)
        {
            modeloActual = (OrdersViewModel)Session["modeloActual"];

            if (modeloActual.orderDetail != null)
            {
                if (!string.IsNullOrEmpty(txtIdOrder))
                {
                    if (action == "Editar")
                    {

                        if (model.GetOrder(int.Parse(txtIdOrder)) != null)
                        {
                            if (stockDisponible(model.GetOrder(int.Parse(txtIdOrder))))
                            {
                                if (model.updateOrderStatus(int.Parse(txtIdOrder), int.Parse(txtLanguage)))
                                {
                                    foreach (var order in modeloActual.allOrders)
                                    {
                                        if (order.idOrder == int.Parse(txtIdOrder))
                                        {

                                            OrderStatus status = model.GetOrderStatus(int.Parse(txtLanguage));
                                            order.orderStatus = status.idStatus;

                                            order.status = status;
                                            if (int.Parse(txtLanguage) == 2)
                                            {
                                                foreach (var producto in order.orderDetails)
                                                {
                                                    var cantidad = producto.product.stock - producto.Units;
                                                    producto.product.stock = cantidad;
                                                    ActualizarProducto(producto.product);
                                                }
                                            }

                                        }
                                    }
                                    MessageBox.Show("Se ha actualizado el pedido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    modeloActual.orderPaged = modeloActual.allOrders.ToPagedList(1, 5);
                                }
                            }
                            else { 
                                    MessageBox.Show("Error: No se puede aceptar el pedido, debido que no hay stock de algunos productos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }

                        return View("Orders", modeloActual);
                    }
                    else if (action == "Eliminar")
                    {

                        if (model.GetOrder(int.Parse(txtIdOrder)) != null)
                        {
                            Order order = model.GetOrder(int.Parse(txtIdOrder));

                            if (order.orderStatus == 1 || order.orderStatus == 2)
                            {
                                if (model.deleteOrder(int.Parse(txtIdOrder)))
                                {
                                    MessageBox.Show("Se ha eliminado el pedido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    modeloActual.order = new Order();
                                    modeloActual.allOrders = model.GetAllOrder();
                                    modeloActual.orderPaged = modeloActual.allOrders.ToPagedList(1, 5);
                                    modeloActual.allOrdersDetail = new List<OrderDetail>();
                                    modeloActual.orderDetailPaged = modeloActual.allOrdersDetail.ToPagedList(1, 5);
                                    modeloActual.orderDetail = null;
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }
                        }
                        Session["modeloActual"] = modeloActual;

                        return View("Orders", modeloActual);

                    }
                }

            }
            else
            {
                ViewBag.ErrorMessageDescription = $"Error: Debes seleccionar una descripción ";
            }
            return RedirectToAction("Orders", "Orders");
        }
      
        public ActionResult EditarOrder(int id)
        {
            Order order = model.GetOrder(id);
            modeloActual = (OrdersViewModel)Session["modeloActual"];
            modeloActual.order = order;
            modeloActual.allOrdersDetail = order.orderDetails;
            modeloActual.orderDetailPaged = modeloActual.allOrdersDetail.ToPagedList(1, 10);
            modeloActual.user = model.GetUser(order.idUser);
            modeloActual.orderStatus = order.status;
            modeloActual.order.status = model.GetOrderStatus(order.orderStatus);


            Session["modeloActual"] = modeloActual;

            return View("Orders", modeloActual);

        } 

        public ActionResult EditarDescripciones(int id)
        {


            return View("Orders", modeloActual);

        }
        private bool ActualizarProducto(Product producto)
        {
            bool estaActualizado = false;

            if (model.UpdateProduct(producto))
            {
                estaActualizado = true;
            }
            else
            {
                MessageBox.Show("Error: No se ha podido actualizar", "Atención");
            }
            return estaActualizado;
        }
        private bool stockDisponible(Order order)
        {
            bool resultado = true;
            if (order.orderStatus == 1)
            {
                foreach (var pedidos in order.orderDetails)
                {
                    if (pedidos.Units > pedidos.product.stock)
                    {
                        resultado = false;
                    }
                }
            }
        
        return resultado;
        }
    }
   

}
