using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Book : BaseEntity
    {
        public static IEnumerable<string> BookCategories = new List<string> { "Adventure", "Classic", "Novel", "Drama", "Fantasy", "Humor", "Mythology", "Romance", "Sci-Fi" };
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public IEnumerable<CategoriesInBook> CategoriesInBook { get; set; }
        public Book()
        {
            CategoriesInBook = new List<CategoriesInBook>();
        }
        public Book(string name, string description, Author author, IEnumerable<CategoriesInBook> categoriesInBooks)
        {
            Name = name;
            Description = description;
            Author = author;
            AuthorId = author.Id;
            CategoriesInBook = categoriesInBooks;
        }
        public Book(string name, string description, Guid authorId, IEnumerable<CategoriesInBook> categoriesInBooks)
        {
            Name = name;
            Description = description;
            AuthorId = authorId;
            CategoriesInBook = categoriesInBooks;
        }
    }
}
