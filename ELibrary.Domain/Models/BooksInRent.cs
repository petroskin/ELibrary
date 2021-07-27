using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class BooksInRent : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid RentId { get; set; }
        public Rent Rent { get; set; }
    }
}
