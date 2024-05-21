using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.Entities
{
    public class OrderDetail
    {
        public int idOrder { get; set; }
        public int idProduct { get; set; }
        public decimal Price { get; set; }
        public int Units { get; set; }
        public Product product { get; set; }
    }
}