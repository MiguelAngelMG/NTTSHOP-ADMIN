using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using System.Web.Services.Description;
using System.Runtime.InteropServices;
using Microsoft.Ajax.Utilities;

namespace NTTShopAdmin.Models
{
    public class ModelDAC
    {
        private string baseUrl = "https://localhost:7077/api/";
        #region Peticiones Users
        public User GetUser(int idUser)
        {
            User user = new User();
            try
            {
                string url = baseUrl + "Users/getUser/" + idUser;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    user = json["user"].ToObject<User>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los idiomas: {ex.Message}");
            }

            return user;
        }
        public List<User> GetAllUser()
        {
            List<User> usuarios = new List<User>();

            try
            {
                string url = baseUrl + "Users/getAllUsers";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var usuarioArray = json["usersList"].ToObject<JArray>();
                    usuarios = usuarioArray.ToObject<List<User>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
            }

            return usuarios;
        }
        public bool UpdateUser(User user, out string message)
        {
            bool insertado = false;
            message = "";
 
            var userData = new { user = user };

            string jsonData = JsonConvert.SerializeObject(userData);

            string url = baseUrl + "Users/updateUser";

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;
                message = httpResponse.StatusDescription;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                        message = "Error de la API: " + errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
                message = ex.Message;
            }

            return insertado;
        }
        public bool DeleteUser(int id)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Users/deleteUser/" + id;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }
        #endregion
        #region Peticiones ManagementUser
        public bool InsertManagementUser(ManagementUser user)
        {
            bool insertado = false;

            var userDATA = new { user = user };

            string jsonData = JsonConvert.SerializeObject(userDATA);

            string url = baseUrl + "ManagementUsers/insertManagementUser";

            try
            {

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar usuario: " + ex.Message);
            }

            return insertado;
        }
        public List<ManagementUser> GetAllManagementUser()
        {
            List<ManagementUser> usuarios = new List<ManagementUser>();

            try
            {
                string url = baseUrl + "ManagementUsers/getAllManagementUsers";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var usuarioArray = json["usersList"].ToObject<JArray>();
                    usuarios = usuarioArray.ToObject<List<ManagementUser>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los Management Users: {ex.Message}");
            }

            return usuarios;
        }

        public ManagementUser GetUserManagm(int idUser)
        {
            ManagementUser user = new ManagementUser();
            try
            {

                var userData = new { user = user };

                string jsonData = JsonConvert.SerializeObject(userData);

                string url = baseUrl + "ManagementUsers/getManagementUser/" + idUser;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    user = json["getUser"].ToObject<ManagementUser>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los idiomas: {ex.Message}");
            }

            return user;
        }

        public bool UpdateUserManag(ManagementUser user, out string message)
        {
            bool insertado = false;
            message = "";

            var userData = new { user = user };

            string jsonData = JsonConvert.SerializeObject(userData);

            string url = baseUrl + "ManagementUsers/updateManagementUser";

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;
                message = httpResponse.StatusDescription;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                        message = "Error de la API: " + errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
                message = ex.Message;
            }

            return insertado;
        }
        public bool DeleteUserManag(int id)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "ManagementUsers/deleteManagementUser/" + id;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }
        #endregion
        #region Peticiones Rate
        public Rate GetRate(int idRate)
        {
            Rate rateResult = new Rate();
            try
            {
                string url = baseUrl + "Rates/getRate/" + idRate;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    rateResult = json["getRates"].ToObject<Rate>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el rate: {ex.Message}");
            }

            return rateResult;
        }

        public List<Rate> GetAllRate()
        {
            List<Rate> rate = new List<Rate>();

            try
            {
                string url = baseUrl + "Rates/getAllRates";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var usuarioArray = json["ratesList"].ToObject<JArray>();
                    rate = usuarioArray.ToObject<List<Rate>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los rate: {ex.Message}");
            }

            return rate;
        }
        #endregion
        #region Orders

        public bool InsertOrder(Order order)
        {
            bool insertado = false;

            var orderDATA = new { order = order };

            string jsonData = JsonConvert.SerializeObject(orderDATA);

            string url = baseUrl + "Orders/insertOrder";

            try
            {

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return insertado;
        }

        public List<Order> GetAllOrder()
        {
            List<Order> orders = new List<Order>();

            try
            {
                string url = baseUrl + "Orders/getAllOrders";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var ordertArray = json["ordersList"].ToObject<JArray>();
                    orders = ordertArray.ToObject<List<Order>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los pedidos: {ex.Message}");
            }

            return orders;
        } 
        public List<Order> GetAllOrder(int stado, string desde, string hasta)
        {
            List<Order> orders = new List<Order>();

            try
            {
                string url = baseUrl + "Orders/getAllOrders";
                if(stado >= 0 || desde != null || hasta != null)
                {
                    url += "?";
                    if (!desde.IsNullOrWhiteSpace())
                    {
                        
                        url += "fromDate=" + desde;
                    }
                    if (!hasta.IsNullOrWhiteSpace())
                    {
                        if(!desde.IsNullOrWhiteSpace())
                        {
                            url += "&toDate=" + hasta;
                        }
                        else
                        {
                            url += "toDate=" + hasta;
                        }
                        
                    }
                    if (stado >= 0)
                    {
                        if (!desde.IsNullOrWhiteSpace() || !hasta.IsNullOrWhiteSpace())
                        {
                            url += "&orderStatus=" + stado;
                        }
                        else
                        {
                            url += "orderStatus=" + stado;
                        }
                       
                    }
                }
               
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var ordertArray = json["ordersList"].ToObject<JArray>();
                    orders = ordertArray.ToObject<List<Order>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los pedidos: {ex.Message}");
            }

            return orders;
        }
        public Order GetOrder(int idOrder)
        {
            Order orders = new Order();
            try
            {

                string url = baseUrl + "Orders/getOrder/" + idOrder;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    orders = json["order"].ToObject<Order>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el order: {ex.Message}");
            }

            return orders;
        }
       
        public bool updateOrderStatus(int idOrder, int idStatus)
        {
            bool insertado = false;        

            string url = baseUrl + "Orders/UpdateOrderStatus/" + idOrder + "/" + idStatus;

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                   
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                      
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return insertado;
        }
        public bool deleteOrder(int idOrder)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Orders/deleteOrder/" + idOrder;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }

        #endregion
        #region OrdersDetails
        #endregion
        #region OrdersStatus
        public OrderStatus GetOrderStatus(int idStatus)
        {
            OrderStatus status = new OrderStatus();
            try
            {

                string url = baseUrl + "Orders/getOrderStatus/" + idStatus;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    status = json["status"].ToObject<OrderStatus>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el estado del pedido: {ex.Message}");
            }

            return status;
        }
        public List<OrderStatus> GetAllStatus()
        {

            List<OrderStatus> status = new List<OrderStatus>();

            try
            {
                string url = baseUrl + "Orders/getAllOrdersStatus";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var usuarioArray = json["ordersList"].ToObject<JArray>();
                    status = usuarioArray.ToObject<List<OrderStatus>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los status: {ex.Message}");
            }

            return status;
        }
        #endregion
        #region Peticiones Products

        public List<Product> GetAllProducts(int language)
        {
            List<Product> products = new List<Product>();

            try
            {
                string url = baseUrl + "Products/getAllProducts/" + language;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var productArray = json["productsList"].ToObject<JArray>();
                    products = productArray.ToObject<List<Product>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos: {ex.Message}");
            }

            return products;
        }
        public Product GetProduct(int idProduct)
        {
            Product product = new Product();
            try
            {
                int language = -1;
                string url = baseUrl + "Products/getProduct/" + idProduct + "/" + language;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    product = json["getProduct"].ToObject<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el producto: {ex.Message}");
            }

            return product;
        }
        public bool InsertProduct(Product product, out int idProduct)
        {
            bool insertado = false;
            idProduct = 0;
            var userData = new { product = product };

            string jsonData = JsonConvert.SerializeObject(userData);

            string url = baseUrl + "Products/InsertProduct";

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;
         
                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var json = JObject.Parse(result);
                        idProduct = json["idProduct"].ToObject<int>();

                    }

                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);

            }

            return insertado;
        }
        public bool UpdateProduct(Product product)
        {
            bool insertado = false;

            var userData = new { product = product };

            string jsonData = JsonConvert.SerializeObject(userData);

            string url = baseUrl + "Products/updateProduct";

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;
                

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                   
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
              
            }

            return insertado;
        }

        public bool UpdateProductRate(ProductRates product)
        {
            bool insertado = false;

            string url = baseUrl + "Products/setPrice/" + product.idProduct + "/" + product.idRate + "/" + product.price;

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;


                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);

            }

            return insertado;
        }

        public bool DeleteProductDescription(int id, int idDescription)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Products/DeleteProductDescription/" + id + "/" + idDescription;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        } 
        
        public bool DeleteProductRate(int id, int idRate)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Products/DeleteProductRate/" + id + "/" + idRate;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }
        public bool DeleteProduct(int id)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Products/DeleteProduct/" + id;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }
        #endregion
        #region Peticiones Language
        public List<Language> GetAllLanguage()
        {
            List<Language> languages = new List<Language>();

            try
            {
                string url = baseUrl + "Language/getAllLanguages";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    var languageArray = json["languageList"].ToObject<JArray>();
                    languages = languageArray.ToObject<List<Language>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los idiomas: {ex.Message}");
            }

            return languages;
        }

        public bool UpdateLanguage(Language language, out string message)
        {
            bool insertado = false;
            message = "";
            // esto nos permite poder poner lo de Language{
            //   idLanguage = "", descripcion = "", iso "" }
            var languageData = new { language = language };

            string jsonData = JsonConvert.SerializeObject(languageData);

            string url = baseUrl + "Language/updateLanguage";

            try
            {
                //HTTP put
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "PUT";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;
                message = httpResponse.StatusDescription;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                        message = "Error de la API: " + errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
                message = ex.Message;
            }

            return insertado;
        }

        public bool DeleteLanguage(int id)
        {
            bool eliminado = false;

            try
            {

                string url = baseUrl + "Language/deleteLanguage/" + id;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                httpRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    eliminado = true;
                }
                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return eliminado;
        }

        public bool InsertLanguage(Language language)
        {
            bool insertado = false;

            // esto nos permite poder poner lo de Language{
            //   idLanguage = "", descripcion = "", iso "" }
            var languageData = new { language = language };

            string jsonData = JsonConvert.SerializeObject(languageData);

            string url = baseUrl + "Language/insertLanguage";

            try
            {
                //HTTP POST
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                // Escribimos el cuerpo del mensaje
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                HttpStatusCode statusCode = httpResponse.StatusCode;

                // Si es OK
                if (statusCode == HttpStatusCode.OK)
                {
                    insertado = true;
                }
                else
                {
                    // Si hay un error, leer el mensaje de error 
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorMessage = streamReader.ReadToEnd();
                        Console.WriteLine("Error de la API: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
            }

            return insertado;
        }

        public Language GetLanguage(int idLanguage)
        {
            Language language = new Language();
            try
            {

                string url = baseUrl + "Language/getLanguage/" + idLanguage;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var json = JObject.Parse(result);
                    language = json["getLanguage"].ToObject<Language>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el order: {ex.Message}");
            }

            return language;
        }


        #endregion
    }
}