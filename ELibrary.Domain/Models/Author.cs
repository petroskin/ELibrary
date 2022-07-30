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
        public string ImageLink { get; set; }
        public IEnumerable<BookAuthor> Books { get; set; }
        public Author()
        {
            Books = new List<BookAuthor>();
        }
        public Author(string name, string surname, string country, string imageLink)
        {
            Name = name;
            Surname = surname;
            Country = country;
            ImageLink = imageLink;
            Books = new List<BookAuthor>();
        }
        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}
