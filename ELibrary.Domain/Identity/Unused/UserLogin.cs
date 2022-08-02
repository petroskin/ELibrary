using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity.Unused
{
    [Table("userlogin", Schema = "elibrary")]
    public class UserLogin : IdentityUserLogin<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("loginprovider")]
        public override string LoginProvider { get => base.LoginProvider; set => base.LoginProvider = value; }
        [Column("providerdisplayname")]
        public override string ProviderDisplayName { get => base.ProviderDisplayName; set => base.ProviderDisplayName = value; }
        [Column("providerkey")]
        public override string ProviderKey { get => base.ProviderKey; set => base.ProviderKey = value; }
        [Column("userid")]
        public override int UserId { get => base.UserId; set => base.UserId = value; }
    }
}
