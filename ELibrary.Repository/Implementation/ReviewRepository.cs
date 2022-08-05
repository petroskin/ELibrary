using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Implementation
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Review> _entities;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Reviews;
        }
        public async Task Delete(Review entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM review WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task Delete(int id)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM review WHERE id = {id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }

        public async Task<Review> Get(int id)
        {
            Review r = await _entities.FromSqlInterpolated($"SELECT * FROM review r WHERE r.id = {id}").FirstOrDefaultAsync();
            if (r == null)
            {
                throw new Exception("Entity not found.");
            }
            return r;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            List<Review> r = await _entities.FromSqlInterpolated($"SELECT * FROM review r").ToListAsync();
            return r;
        }

        public async Task<IEnumerable<Review>> GetAllByBook(int id)
        {
            List<Review> r = await _entities.FromSqlInterpolated($"SELECT * FROM review r WHERE r.bookid = {id}").ToListAsync();
            return r;
        }

        public async Task<IEnumerable<Review>> GetAllByUser(int id)
        {
            List<Review> r = await _entities.FromSqlInterpolated($"SELECT * FROM review r WHERE r.elibuserid = {id}").ToListAsync();
            return r;
        }

        public async Task Insert(Review entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO review (elibuserid, bookid, rating, \"comment\") VALUES ({entity.UserId}, {entity.BookId}, {entity.Rating}, {entity.Comment})");
        }

        public async Task Update(Review entity)
        {
            int affectedCount = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE review SET elibuserid = {entity.UserId}, bookid = {entity.BookId}, rating = {entity.Rating}, \"comment\" = {entity.Comment} WHERE id = {entity.Id}");
            if (affectedCount == 0)
            {
                throw new Exception("Entity not found.");
            }
        }
    }
}
