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
    public class RentRepository : IRentRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Rent> _entities;
        string errorMessage = string.Empty;

        public RentRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Rent>();
        }
        public async Task Delete(Rent entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Rent> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Rent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Rent>> GetAll(string userId)
        {
            List<Rent> r = await _entities.FromSqlInterpolated($"SELECT * FROM rent r WHERE r.elibuserid = {int.Parse(userId)}").ToListAsync();
            return r;
        }

        public async Task<IEnumerable<Rent>> GetAllCurrent(string userId)
        {
            List<Rent> r = await _entities.FromSqlInterpolated($"SELECT * FROM rent r WHERE r.elibuserid = {int.Parse(userId)} AND r.subscriptionend >= current_date").ToListAsync();
            return r;
        }

        public async Task Insert(Rent entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO rent (elibuserid, bookid, subscriptionstart, subscriptionend) VALUES ({entity.UserId}, {entity.BookId}, {entity.Start}, {entity.End})");
        }

        public async Task Update(Rent entity)
        {
            throw new NotImplementedException();
        }
    }
}
