using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public void RemoveRoles(ELibraryUserDto dto)
        {
            _userRepository.RemoveRoles(dto);
        }

        public void Update(ELibraryUser entity)
        {
            _userRepository.Update(entity);
        }
    }
}
