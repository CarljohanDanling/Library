﻿using Library.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.ViewModels
{
    public class LibraryItemViewModel
    {
        public List<LibraryItemModel> LibraryItems { get; set; }
        public LibraryItemModel LibraryItemModel{ get; set; }
    }
}