using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity
{
    [Table("userrole", Schema = "elibrary")]
    public class UserRole : IdentityUserRole<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("elibuserid")]
        public override int UserId { get => base.UserId; set => base.UserId = value; }
        public ELibraryUser User { get; set; }
        [Column("elibroleid")]
        public override int RoleId { get => base.RoleId; set => base.RoleId = value; }
        public ELibraryRole Role { get; set; }
        [Column("datestart")]
        public DateTime DateStart { get; set; }
        [Column("dateend")]
        public DateTime DateEnd { get; set; }
        public UserRole() : base()
        {

        }
        public UserRole(int userId, int roleId, DateTime dateStart, DateTime dateEnd) : base()
        {
            UserId = userId;
            RoleId = roleId;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
