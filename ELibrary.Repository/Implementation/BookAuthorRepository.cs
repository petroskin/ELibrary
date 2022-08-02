using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<BookAuthor> _entities;

        public BookAuthorRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.BookAuthors;
        }
        public async Task Delete(BookAuthor entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM bookauthors WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM bookauthors WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<BookAuthor> Get(int id)
        {
            BookAuthor ba = await _entities.FromSqlInterpolated($"SELECT * FROM bookauthors ba WHERE ba.id = {id}").FirstOrDefaultAsync();
            if (ba == null)
            {
                throw new Exception("Entity not found.");
            }
            return ba;
        }

        public async Task<IEnumerable<BookAuthor>> GetAll()
        {
            List<BookAuthor> ba = await _entities.FromSqlInterpolated($"SELECT * FROM bookauthors ba").ToListAsync();
            return ba;
        }

        public async Task<IEnumerable<BookAuthor>> GetByAuthorId(int id)
        {
            List<BookAuthor> ba = await _entities.FromSqlInterpolated($"SELECT * FROM bookauthors ba WHERE ba.authorid = {id}").ToListAsync();
            return ba;
        }

        public async Task<IEnumerable<BookAuthor>> GetByBookId(int id)
        {
            List<BookAuthor> ba = await _entities.FromSqlInterpolated($"SELECT * FROM bookauthors ba WHERE ba.bookid = {id}").ToListAsync();
            return ba;
        }

        public async Task Insert(BookAuthor entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO bookauthors (authorid, bookid) VALUES ({entity.AuthorId}, {entity.BookId})");
        }

        public async Task Update(BookAuthor entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE bookauthors SET authorid = {entity.AuthorId}, bookid = {entity.BookId} WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
