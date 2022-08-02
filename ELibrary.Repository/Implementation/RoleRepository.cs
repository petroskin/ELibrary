using ELibrary.Domain.Identity;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ELibraryRole> _entities;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<ELibraryRole>();
        }
        public async Task<ELibraryRole> Get(int id)
        {
            ELibraryRole role = await _entities.FromSqlInterpolated($"SELECT * FROM elibrole e WHERE e.id = {id}").FirstOrDefaultAsync();
            return role;
        }
    }
}
