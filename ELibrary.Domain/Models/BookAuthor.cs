using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class BookAuthor : BaseEntity
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
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
