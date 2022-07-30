using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<CategoriesInBook> Books { get; set; }
        public Category()
        {
            Books = new List<CategoriesInBook>();
        }
        public Category(string name)
        {
            Name = name;
            Books = new List<CategoriesInBook>();
        }
    }
}
