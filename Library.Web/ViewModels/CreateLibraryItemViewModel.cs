using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class CreateLibraryItemViewModel
    {
        public DigitalMediaItem DigitalMedia { get; set; }
        public NonDigitalMediaItem NonDigitalMedia { get; set; }


        public AudioBookCreate AudioBook { get; set; }
        public BookCreate Book { get; set; }
        public DvdCreate Dvd { get; set; }
        public ReferenceBookCreate ReferenceBook { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public MediaItemCategory MediaItemCategory { get; set; }
    }
}