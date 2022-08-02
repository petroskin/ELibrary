using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity.Unused
{
    [Table("usertoken", Schema = "elibrary")]
    public class UserToken : IdentityUserToken<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userid")]
        public override int UserId { get => base.UserId; set => base.UserId = value; }
        [Column("loginprovider")]
        public override string LoginProvider { get => base.LoginProvider; set => base.LoginProvider = value; }
        [Column("name")]
        public override string Name { get => base.Name; set => base.Name = value; }
        [Column("value")]
        public override string Value { get => base.Value; set => base.Value = value; }
    }
}
