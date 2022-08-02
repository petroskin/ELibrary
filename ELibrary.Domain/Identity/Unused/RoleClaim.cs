using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity.Unused
{
    [Table("roleclaim", Schema = "elibrary")]
    public class RoleClaim : IdentityRoleClaim<int>
    {
        [Key]
        [Column("id")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Column("roleid")]
        public override int RoleId { get => base.RoleId; set => base.RoleId = value; }
        [Column("claimtype")]
        public override string ClaimType { get => base.ClaimType; set => base.ClaimType = value; }
        [Column("claimvalue")]
        public override string ClaimValue { get => base.ClaimValue; set => base.ClaimValue = value; }
    }
}
