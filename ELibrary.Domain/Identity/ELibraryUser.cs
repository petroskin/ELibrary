using ELibrary.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity
{
    [Table("elibuser", Schema = "elibrary")]
    public class ELibraryUser : IdentityUser<int>
    {
        // Roles: "Standard", "Premium", "Admin"
        public static int BooksAllowedForStandard = 3;

        [Key]
        [Column("id")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("email")]
        public override string Email { get => base.Email; set => base.Email = value; }
        [Column("password")]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        public IEnumerable<UserRole> Roles { get; set; }
        public Cart UserCart { get; set; }
        public IEnumerable<Rent> Rents { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        [Column("lockoutend")]
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }
        [Column("twofactorenabled")]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
        [Column("phonenumberconfirmed")]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
        [Column("phonenumber")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        [Column("concurrencystamp")]
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        [Column("securitystamp")]
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
        [Column("emailconfirmed")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }
        [Column("normalizedemail")]
        public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }
        [Column("normalizedusername")]
        public override string NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }
        [Column("username")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        [Column("lockoutenabled")]
        public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }
        [Column("accessfailedcount")]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }

        public ELibraryUser() : base()
        {
            Roles = new List<UserRole>();
            Rents = new List<Rent>();
            Reviews = new List<Review>();
        }

        public ELibraryUser(string name, string surname, string email) : base(email)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Roles = new List<UserRole>();
            Rents = new List<Rent>();
            Reviews = new List<Review>();
        }
    }
}
