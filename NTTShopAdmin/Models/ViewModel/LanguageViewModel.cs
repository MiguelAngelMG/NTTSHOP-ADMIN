using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.ViewModel
{
    public class LanguageViewModel
    {

        public IPagedList<Language> Language { get; set; }

        public string idLanguage { get; set; }
        public string descripcion { get; set; }
        public string iso { get; set; }

    }
}