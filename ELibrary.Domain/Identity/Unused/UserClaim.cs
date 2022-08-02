using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity.Unused
{
    [Table("userclaim", Schema = "elibrary")]
    public class UserClaim : IdentityUserClaim<int>
    {
        [Key]
        [Column("id")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Column("userid")]
        public override int UserId { get => base.UserId; set => base.UserId = value; }
        [Column("claimtype")]
        public override string ClaimType { get => base.ClaimType; set => base.ClaimType = value; }
        [Column("claimvalue")]
        public override string ClaimValue { get => base.ClaimValue; set => base.ClaimValue = value; }
    }
}
