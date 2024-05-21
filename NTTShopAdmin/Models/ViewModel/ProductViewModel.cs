using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel() { 
            product = new Product();
            productDescription = new ProductDescription();
            productRates = new ProductRates();
            allRates = new List<Rate>();
        }
        public IPagedList<Product> productsList { get; set; }
        public IPagedList<ProductDescription> descriptionsList { get; set; }
        public IPagedList<ProductRates> ratesList { get; set; }
        public List<Product> allProducts { get; set; } = new List<Product>();
        public List<ProductDescription> allProductsDescription { get; set; } = new List<ProductDescription>();
        public List<ProductRates> allProductsRate { get; set; } = new List<ProductRates>();

        public List<Rate> allRates { get; set; }
        public List<Language> languagesList { get; set; } = new List<Language>();
        public Product product  { get; set; }
        public ProductDescription productDescription { get; set; }
        public ProductRates productRates { get; set;}
    }
       
}