using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity
{
    [Table("elibrole", Schema = "elibrary")]
    public class ELibraryRole : IdentityRole<int>
    {
        [Key]
        [Column("id")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Column("name")]
        public override string Name { get => base.Name; set => base.Name = value; }
        [Column("normalizedname")]
        public override string NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
        [Column("concurrencystamp")]
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        [Column("price")]
        public decimal Price { get; set; }
        public IEnumerable<UserRole> Users { get; set; }
        public ELibraryRole() : base()
        {
            Users = new List<UserRole>();
        }
        public ELibraryRole(string name, decimal price) : base(name)
        {
            Price = price;
            Users = new List<UserRole>();
        }
    }
}
