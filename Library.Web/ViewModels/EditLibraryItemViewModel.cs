using Library.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.ViewModels
{
    public class EditLibraryItemViewModel
    {
        public AudioBookEdit AudioBook { get; set; }
        public BookEdit Book { get; set; }
        //public DvdEdit DvdEdit{ get; set; }
        //public ReferenceBookEdit ReferenceBookEdit { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}
