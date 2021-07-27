using ELibrary.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Identity
{
    public class ELibraryUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Cart UserCart { get; set; }
    }
}
