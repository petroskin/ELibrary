using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("review", Schema = "elibrary")]
    public class Review : BaseEntity
    {
        [Column("elibuserid")]
        public int UserId { get; set; }
        public ELibraryUser User { get; set; }
        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Column("rating")]
        public int Rating { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        public Review()
        {

        }
        public Review(int userId, int bookId, int rating, string comment)
        {
            UserId = userId;
            BookId = bookId;
            Rating = rating;
            Comment = comment;
        }
        public Review(ELibraryUser user, Book book, int rating, string comment) : this(user.Id, book.Id, rating, comment)
        {
            User = user;
            Book = book;
        }
    }
}
