using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        public IEnumerable<BooksInCart> BooksInCart { get; set; }
    }
}
