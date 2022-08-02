using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Cart> _entities;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Carts;
        }

        public async Task Create(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO cart (elibuserid) VALUES ({id})");
        }

        public async Task<Cart> Get(int id)
        {
            Cart c = await _entities.FromSqlInterpolated($"SELECT * FROM cart c WHERE c.elibuserid = {id}").FirstOrDefaultAsync();
            if (c == null)
            {
                throw new Exception("Entity not found.");
            }
            return c;
        }
    }
}
