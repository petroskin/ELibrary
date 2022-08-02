using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("rent", Schema = "elibrary")]
    public class Rent : BaseEntity
    {
        [Column("elibuserid")]
        public int UserId { get; set; }
        public ELibraryUser User { get; set; }
        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Column("subscriptionstart")]
        public DateTime Start { get; set; }
        [Column("subscriptionend")]
        public DateTime End { get; set; }
        public string GetDateFormat()
        {
            return $"{Start:dd MMMM yyyy} - {End:dd MMMM yyyy}";
        }
        public Rent()
        {

        }
        public Rent(int userId, int bookId)
        {
            UserId = userId;
            BookId = bookId;
            Start = DateTime.Now;
            End = DateTime.Now.AddDays(30);
        }
        public Rent(ELibraryUser user, Book book) : this(user.Id, book.Id)
        {
            User = user;
            Book = book;
        }
    }
}
