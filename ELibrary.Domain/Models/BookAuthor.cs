using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("bookauthors", Schema = "elibrary")]
    public class BookAuthor : BaseEntity
    {
        [Column("authorid")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        public BookAuthor()
        {

        }
        public BookAuthor(int authorId, int bookId)
        {
            AuthorId = authorId;
            BookId = bookId;
        }
        public BookAuthor(Author author, Book book)
        {
            Author = author;
            AuthorId = author.Id;
            Book = book;
            BookId = book.Id;
        }
    }
}
