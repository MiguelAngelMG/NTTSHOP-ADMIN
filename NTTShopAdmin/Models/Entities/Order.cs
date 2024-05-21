using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.Entities
{
    public class Order
    {
        ModelDAC model = new ModelDAC();
        public Order()
        {
            orderDetails = new List<OrderDetail>();
        }
        public int idOrder { get; set; }
        public int idUser { get; set; }
        public DateTime orderDate { get; set; }
        public int orderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus status { get; set; }

        public List<OrderDetail> orderDetails { get; set; }

        public void calcularPriceTotal()
        {
            if (orderDetails.Count > 0)
            {
                TotalPrice = 0;
                foreach (OrderDetail detail in orderDetails)
                {
                    TotalPrice += detail.Price * detail.Units;

                }
            }
        }

        public string userName()
        {
            string name = "";
            
            User user = model.GetUser(idUser);
            name = user.Name + " " + user.Surname1 + " " + user.Surname2;

            return name;
        }
    }
}
