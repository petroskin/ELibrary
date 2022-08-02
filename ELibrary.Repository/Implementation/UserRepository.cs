using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ELibraryUser> _entities;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<ELibraryUser>();
        }

        public async Task<ELibraryUser> GetLatest()
        {
            ELibraryUser user = await _entities.FromSqlInterpolated($"SELECT * FROM elibuser e ORDER BY id DESC LIMIT 1").FirstOrDefaultAsync();
            return user;
        }
    }
}
