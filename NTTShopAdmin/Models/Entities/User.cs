using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.Entities
{
    public class User
    {
        public int PkUser { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname1 { get; set; }

        public string Surname2 { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string Town { get; set; }

        public string PostalCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Languages { get; set; }

        public int Rate { get; set; }
    }
}