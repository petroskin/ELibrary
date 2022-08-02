using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class BookCartRepository : IBookCartRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<BooksInCart> _entities;

        public BookCartRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.BooksInCarts;
        }
        public async Task Add(int userId, int bookId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO booksincart (bookid, cartuserid) VALUES ({bookId}, {userId})");
        }

        public async Task<IEnumerable<BooksInCart>> GetByCart(int cartId)
        {
            List<BooksInCart> list = await _entities.FromSqlInterpolated($"SELECT * FROM booksincart bc WHERE bc.cartuserid = {cartId}").ToListAsync();
            return list;
        }

        public async Task<bool> IsPresent(int cartId, int bookId)
        {
            List<BooksInCart> list = await _entities.FromSqlInterpolated($"SELECT * FROM booksincart bc WHERE bc.bookid = {bookId} AND bc.cartuserid = {cartId}").ToListAsync();
            return list.Count != 0;
        }

        public async Task Remove(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM booksincart WHERE id = {id}");
        }

        public async Task RemoveAllFromCart(int cartId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM booksincart WHERE cartuserid = {cartId}");
        }
    }
}
