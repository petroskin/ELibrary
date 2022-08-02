using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IUserRoleRepository
    {
        Task ChangeUserRole(int userId, int roleId);
        Task<IEnumerable<UserRole>> GetForUser(int userId);
    }
}
