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
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Book> _entities;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Book;
        }
        public async Task Delete(Book entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM book WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM book WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<Book> Get(int id)
        {
            Book b = await _entities.FromSqlInterpolated($"SELECT * FROM book b WHERE b.id = {id}").FirstOrDefaultAsync();
            if (b == null)
            {
                throw new Exception("Entity not found.");
            }
            return b;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            List<Book> b = await _entities.FromSqlInterpolated($"SELECT * FROM book b").ToListAsync();
            return b;
        }

        public async Task<Book> GetWithAuthorsCategoriesPublisher(int id)
        {
            Book b = await _entities.FromSqlInterpolated($"SELECT * FROM book b WHERE b.id = {id}")
                .Include(b => b.Publisher)
                .Include(b => b.Categories).ThenInclude(bc => bc.Category)
                .Include(b => b.Authors).ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync();
            if (b == null)
            {
                throw new Exception("Entity not found.");
            }
            return b;
        }

        public async Task Insert(Book entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO book (\"name\", description, imagelink, publisherid, totalrents, avgrating) VALUES ('{entity.Name}', '{entity.Description}', '{entity.ImageLink}', '{entity.PublisherId}', 0, 0)");
        }

        public async Task Update(Book entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE book SET \"name\" = '{entity.Name}', description = '{entity.Description}', imagelink = '{entity.ImageLink}', publisherid = '{entity.PublisherId}' WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
