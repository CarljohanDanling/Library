using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class EditLibraryItemViewModel
    {
        public AudioBookEdit AudioBook { get; set; }
        public BookEdit Book { get; set; }
        public DvdEdit Dvd { get; set; }
        public ReferenceBookEdit ReferenceBook{ get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}