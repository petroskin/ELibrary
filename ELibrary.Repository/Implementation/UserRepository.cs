using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ELibraryUser> _entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<ELibraryUser>();
        }
        public void Delete(ELibraryUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public ELibraryUser Get(string id)
        {
            return _entities.SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<ELibraryUser> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public ELibraryUserDto GetDto(string id)
        {
            ELibraryUser user = _entities.Include(i => i.Rents).SingleOrDefault(i => i.Id == id);
            if (user == null)
            {
                return null;
            }
            ELibraryUserDto dto = new ELibraryUserDto(user);

            dto.Role = _context.Roles.Where(i => _context.UserRoles.Where(j => j.UserId == id && j.RoleId == i.Id).Any()).SingleOrDefault().Name;
            dto.BooksRented = user.Rents.Count();

            return dto;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public ELibraryUser GetWithCart(string id)
        {
            return _entities.Include(i => i.UserCart).ThenInclude(i => i.BooksInCart).ThenInclude(i => i.Book).ThenInclude(i => i.Author)
                .Include(i => i.UserCart).ThenInclude(i => i.BooksInCart).ThenInclude(i => i.Book).ThenInclude(i => i.CategoriesInBook)
                .SingleOrDefault(i => i.Id == id);
        }

        public void Insert(ELibraryUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void RemoveRoles(ELibraryUserDto dto)
        {
            _context.RemoveRange(_context.UserRoles.Where(i => i.UserId == dto.Id));
        }

        public void Update(ELibraryUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
