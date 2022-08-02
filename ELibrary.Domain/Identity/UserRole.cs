using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Identity
{
    [Table("userrole", Schema = "elibrary")]
    public class UserRole
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("elibuserid")]
        public int UserId { get; set; }
        public ELibraryUser User { get; set; }
        [Column("elibroleid")]
        public int RoleId { get; set; }
        public ELibraryRole Role { get; set; }
        [Column("datestart")]
        public DateTime DateStart { get; set; }
        [Column("dateend")]
        public DateTime? DateEnd { get; set; }
        public UserRole()
        {

        }
        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
            DateStart = DateTime.Now;
            DateEnd = null;
        }
        public UserRole(int userId, int roleId, DateTime dateStart, DateTime dateEnd)
        {
            UserId = userId;
            RoleId = roleId;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
