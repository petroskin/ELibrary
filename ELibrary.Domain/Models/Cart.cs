using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("cart", Schema = "elibrary")]
    public class Cart
    {
        [Key]
        [Column("elibuserid")]
        public int UserId { get; set; }
        public IEnumerable<BooksInCart> BooksInCart { get; set; }
        public Cart()
        {
            BooksInCart = new List<BooksInCart>();
        }
        public Cart(int userId) : this()
        {
            UserId = userId;
        }
    }
}
