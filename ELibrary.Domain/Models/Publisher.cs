using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("publisher", Schema = "elibrary")]
    public class Publisher : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public Publisher()
        {
            Books = new List<Book>();
        }
        public Publisher(string name) : this()
        {
            Name = name;
        }
    }
}
