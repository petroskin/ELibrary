using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public Author()
        {
            Books = new List<Book>();
        }
        public Author(string name, string surname, string country, string image)
        {
            Name = name;
            Surname = surname;
            Country = country;
            Image = image;
            Books = new List<Book>();
        }
        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}
