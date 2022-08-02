using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Category> _entities;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = _context.Category;
        }
        public async Task Delete(Category entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM category WHERE id == {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM category WHERE id == {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
            throw new NotImplementedException();
        }

        public async Task<Category> Get(int id)
        {
            Category c = await _entities.FromSqlInterpolated($"SELECT * FROM category c WHERE c.id = {id}").FirstOrDefaultAsync();
            if (c == null)
            {
                throw new Exception("Entity not found.");
            }
            return c;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            List<Category> c = await _entities.FromSqlInterpolated($"SELECT * FROM category c").ToListAsync();
            return c;
        }

        public async Task Insert(Category entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO category (\"name\") VALUES ({entity.Name})");
        }

        public async Task Update(Category entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE category SET \"name\" = {entity.Name} WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
