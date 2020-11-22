using System.Collections.Generic;

namespace Library.Data.Database.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<LibraryItem> LibraryItems{ get; set; }
    }
}