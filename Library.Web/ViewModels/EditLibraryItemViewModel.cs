using Library.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.ViewModels
{
    public class EditLibraryItemViewModel
    {
        public DigitalMediaItem DigitalMediaItem { get; set; }
        public NonDigitalMediaItem NonDigitalMediaItem { get; set; }

        [Display(Name = "Type")]
        public LibraryItemType LibraryItemType { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}