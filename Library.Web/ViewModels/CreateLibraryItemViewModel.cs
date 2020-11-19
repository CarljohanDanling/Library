using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class CreateLibraryItemViewModel
    {
        public AudioBookCreate AudioBook { get; set; }
        public BookCreate Book { get; set; }
        public DVD DVD { get; set; }
        public ReferenceBook ReferenceBook { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public string SelectedType { get; set; }
    }
}