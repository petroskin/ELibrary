using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ELibraryUser> _userManager;
        public UserService(IUserRepository userRepository, UserManager<ELibraryUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public void ChangeRoles(ELibraryUserDto dto)
        {
            _userRepository.RemoveRoles(dto);
            _userManager.AddToRoleAsync(_userRepository.Get(dto.Id), dto.Role).Wait();
        }

        public void Delete(ELibraryUser entity)
        {
            _userRepository.Delete(entity);
        }

        public ELibraryUser Get(string id)
        {
            return _userRepository.Get(id);
        }

        public IEnumerable<ELibraryUser> GetAll()
        {
            return _userRepository.GetAll();
        }

        public ELibraryUserDto GetDto(string id)
        {
            return _userRepository.GetDto(id);
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _userRepository.GetRoles();
        }

        public ELibraryUser GetWithCart(string id)
        {
            return _userRepository.GetWithCart(id);
        }

        public void Insert(ELibraryUser entity)
        {
            _userRepository.Insert(entity);
        }

        public void InsertFromDtoAsync(IEnumerable<ExcelUserDataDto> entities)
        {
            foreach (ExcelUserDataDto entity in entities)
            {
                if (_userManager.Users.Where(i => i.Email == entity.Email).Any())
                {
                    continue;
                }
                ELibraryUser user = new ELibraryUser
                {
                    UserName = entity.Email,
                    Email = entity.Email,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    UserCart = new Cart(),
                    EmailConfirmed = true
                };
                user.UserCart.UserId = user.Id;
                _userManager.CreateAsync(user, entity.Password).Wait();
                if (entity.Role != "Admin" && entity.Role != "Premium")
                {
                    entity.Role = "Standard";
                }
                _userManager.AddToRoleAsync(user, entity.Role).Wait();
            }
        }

        public void Update(ELibraryUser entity)
        {
            _userRepository.Update(entity);
        }
    }
}
