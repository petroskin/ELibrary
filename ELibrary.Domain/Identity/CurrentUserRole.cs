using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity
{
    [Table("currentuserrole", Schema = "elibrary")]
    public class CurrentUserRole : IdentityUserRole<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("elibuserid")]
        public override int UserId { get => base.UserId; set => base.UserId = value; }
        [Column("elibroleid")]
        public override int RoleId { get => base.RoleId; set => base.RoleId = value; }
    }
}
