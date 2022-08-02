using ELibrary.Domain.Identity;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task ChangeUserRole(int userId, int roleId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL change_subscription_plan({userId}, {roleId})");
        }

        public async Task<IEnumerable<UserRole>> GetForUser(int userId)
        {
            List<UserRole> ur = await _context.AllUserRoles.FromSqlInterpolated($"SELECT * FROM userrole ur WHERE ur.elibuserid = {userId}").ToListAsync();
            return ur;
        }
    }
}
