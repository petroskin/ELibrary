using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class BookCategoriesRepository : IBookCategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<CategoriesInBook> _entities;

        public BookCategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.BookCategories;
        }
        public async Task Delete(CategoriesInBook entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM bookcategories WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM bookcategories WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<CategoriesInBook> Get(int id)
        {
            CategoriesInBook bc = await _entities.FromSqlInterpolated($"SELECT * FROM bookcategories bc WHERE bc.id = {id}").FirstOrDefaultAsync();
            if (bc == null)
            {
                throw new Exception("Entity not found.");
            }
            return bc;
        }

        public async Task<IEnumerable<CategoriesInBook>> GetAll()
        {
            List<CategoriesInBook> bc = await _entities.FromSqlInterpolated($"SELECT * FROM bookcategories bc").ToListAsync();
            return bc;
        }

        public async Task<IEnumerable<CategoriesInBook>> GetByCategoryId(int id)
        {
            List<CategoriesInBook> bc = await _entities.FromSqlInterpolated($"SELECT * FROM bookcategories bc WHERE bc.categoryid = {id}").ToListAsync();
            return bc;
        }

        public async Task<IEnumerable<CategoriesInBook>> GetByBookId(int id)
        {
            List<CategoriesInBook> bc = await _entities.FromSqlInterpolated($"SELECT * FROM bookcategories bc WHERE bc.bookid = {id}").ToListAsync();
            return bc;
        }

        public async Task Insert(CategoriesInBook entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO bookcategories (categoryid, bookid) VALUES ('{entity.CategoryId}', '{entity.BookId}')");
        }

        public async Task Update(CategoriesInBook entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE bookcategories SET categoryid = '{entity.CategoryId}', bookid = '{entity.BookId}' WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
