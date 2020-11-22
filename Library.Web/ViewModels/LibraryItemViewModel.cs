using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class LibraryItemViewModel
    {
        public List<LibraryItemBase> LibraryItems { get; set; }
        public LibraryItemBase LibraryItemBase{ get; set; }
    }
}