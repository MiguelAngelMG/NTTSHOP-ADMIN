using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Entities
{
    public class ManagementUser
    {

        public int PkUser { get; set; }
        [Required]
        public string login { get; set; }
        [Required]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname1 { get; set; } 

        public string Surname2 { get; set; }

        public string Email { get; set; }

        public string Languages { get; set; }
    }
}