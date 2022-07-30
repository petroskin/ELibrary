using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public Publisher()
        {
            Books = new List<Book>();
        }
        public Publisher(string name)
        {
            Name = name;
            Books = new List<Book>();
        }
    }
}
