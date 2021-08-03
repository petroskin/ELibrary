using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class Rent : BaseEntity
    {
        public string UserId { get; set; }
        public ELibraryUser User { get; set; }
        public IEnumerable<BooksInRent> BooksInRent { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string GetDateFormat()
        {
            DateTime dateTime = new DateTime(Year, Month, 1);
            return dateTime.ToString("MMMM yyyy");
        }
    }
}
