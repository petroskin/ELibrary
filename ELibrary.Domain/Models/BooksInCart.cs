using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class BooksInCart : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
