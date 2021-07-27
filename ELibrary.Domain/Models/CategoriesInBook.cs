using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class CategoriesInBook : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string Category { get; set; }
        public CategoriesInBook() { }
        public CategoriesInBook(Book book, string category)
        {
            Book = book;
            BookId = book.Id;
            Category = category;
        }
        public CategoriesInBook(Guid bookId, string category)
        {
            BookId = bookId;
            Category = category;
        }
    }
}
