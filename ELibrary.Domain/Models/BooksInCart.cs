using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("booksincart", Schema = "elibrary")]
    public class BooksInCart : BaseEntity
    {
        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Column("cartuserid")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public BooksInCart()
        {

        }
        public BooksInCart(int bookId, int cartId)
        {
            BookId = bookId;
            CartId = cartId;
        }
        public BooksInCart(Book book, Cart cart) : this(book.Id, cart.UserId)
        {
            Book = book;
            Cart = cart;
        }
    }
}
