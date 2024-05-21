﻿using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.ViewModel
{
    public class NewUserViewModel
    {
        public List<Language> languagesList { get; set; } = new List<Language>();
        public ManagementUser user { get; set; } = new ManagementUser();

    }
       
}