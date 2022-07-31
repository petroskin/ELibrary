using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("category", Schema = "elibrary")]
    public class Category : BaseEntity
    {
        [Column("name")]
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
