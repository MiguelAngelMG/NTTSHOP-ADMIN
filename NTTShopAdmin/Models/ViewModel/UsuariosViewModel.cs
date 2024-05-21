using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.ViewModel
{
    public class UsuariosViewModel
    {
        public IPagedList<ManagementUser> ManagementUsers { get; set; }
        public IPagedList<User> Users { get; set; }
        public List<Language> languagesList { get; set; } = new List<Language>();
        public Language language { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Surname1 { get; set; }
        public string Surname2 { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserIdioma { get; set; }

    }
}