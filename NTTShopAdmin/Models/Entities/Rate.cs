using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.Entities
{
    public class Rate
    {
        public int idRate { get; set; }

        public string descripcion { get; set; }

        public bool defaultRate { get; set; }
    }
}