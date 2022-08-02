using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("author", Schema = "elibrary")]
    public class Author : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("imagelink")]
        public string ImageLink { get; set; }
        public IEnumerable<BookAuthor> Books { get; set; }
        public Author()
        {
            Books = new List<BookAuthor>();
        }
        public Author(string name, string surname, string country, string imageLink) : this()
        {
            Name = name;
            Surname = surname;
            Country = country;
            ImageLink = imageLink;
        }
        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}
