using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("bookcategories", Schema = "elibrary")]
    public class CategoriesInBook : BaseEntity
    {
        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Column("categoryid")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public CategoriesInBook() { }
        public CategoriesInBook(int bookId, int categoryId)
        {
            BookId = bookId;
            CategoryId = categoryId;
        }
        public CategoriesInBook(Book book, Category category)
        {
            Book = book;
            BookId = book.Id;
            Category = category;
            CategoryId = category.Id;
        }
    }
}
