using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.Entities
{
    public class Product
    {
        public int idProduct { get; set; }
        public int stock { get; set; }
        public bool enabled { get; set; }
        public List<ProductDescription> description { get; set; }
        public List<ProductRates> rates { get; set; }

        public Product()
        {
            description = new List<ProductDescription>();
            rates = new List<ProductRates>();
        }
    }
}