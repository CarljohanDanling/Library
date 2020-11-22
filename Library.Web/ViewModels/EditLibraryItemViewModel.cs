using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class EditLibraryItemViewModel
    {
        public DigitalMediaItem DigitalMediaItem { get; set; }
        public NonDigitalMediaItem NonDigitalMediaItem { get; set; }

        public AudioBook AudioBook{ get; set; }
        public Book Book { get; set; }
        public DvdEdit Dvd { get; set; }
        public ReferenceBookEdit ReferenceBook{ get; set; }

        public LibraryItemType LibraryItemType { get; set; }
        public MediaItemCategory MediaItemCategory{ get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}