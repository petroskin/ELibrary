using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Publisher> _entities;

        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = _context.Publisher;
        }
        public async Task Delete(Publisher entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM publisher WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM publisher WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<Publisher> Get(int id)
        {
            Publisher p = await _entities.FromSqlInterpolated($"SELECT * FROM publisher p WHERE p.id = {id}").FirstOrDefaultAsync();
            if (p == null)
            {
                throw new Exception("Entity not found.");
            }
            return p;
        }

        public async Task<IEnumerable<Publisher>> GetAll()
        {
            List<Publisher> p = await _entities.FromSqlInterpolated($"SELECT * FROM publisher p").ToListAsync();
            return p;
        }

        public async Task<Publisher> GetWithBooks(int id)
        {
            Publisher p = await _entities.FromSqlInterpolated($"SELECT * FROM publisher p WHERE p.id = {id}").Include(p => p.Books).FirstOrDefaultAsync();
            if (p == null)
            {
                throw new Exception("Entity not found.");
            }
            return p;
        }

        public async Task Insert(Publisher entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO publisher (\"name\") VALUES ('{entity.Name}')");
        }

        public async Task Update(Publisher entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE publisher SET \"name\" = '{entity.Name}' WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
