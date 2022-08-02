using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Author> _entities;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Author;
        }
        public async Task Delete(Author entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM author WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM author WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<Author> Get(int id)
        {
            Author a = await _entities.FromSqlInterpolated($"SELECT * FROM author a WHERE a.id = {id}").FirstOrDefaultAsync();
            if (a == null)
            {
                throw new Exception("Entity not found.");
            }
            return a;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            List<Author> a = await _entities.FromSqlInterpolated($"SELECT * FROM author a").ToListAsync();
            return a;
        }

        public async Task Insert(Author entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO author (\"name\", surname, country, imagelink) VALUES ({entity.Name}, {entity.Surname}, {entity.Country}, {entity.ImageLink})");
        }

        public async Task Update(Author entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE author SET \"name\" = {entity.Name}, surname = {entity.Surname}, country = {entity.Country}, imagelink = {entity.ImageLink} WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
