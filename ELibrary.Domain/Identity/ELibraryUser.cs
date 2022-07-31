using ELibrary.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Identity
{
    public class ELibraryUser : IdentityUser<int>
    {
        // Roles: "Standard", "Premium", "Admin"
        public static int BooksAllowedForStandard = 3;
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Cart UserCart { get; set; }
        public IEnumerable<Rent> Rents { get; set; }
    }
}
