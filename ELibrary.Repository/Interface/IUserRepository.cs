using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ELibraryUser> GetAll();
        ELibraryUser Get(string id);
        IEnumerable<IdentityRole> GetRoles();
        void RemoveRoles(ELibraryUserDto dto);
        ELibraryUserDto GetDto(string id);
        ELibraryUser GetWithCart(string id);
        void Insert(ELibraryUser entity);
        void Update(ELibraryUser entity);
        void Delete(ELibraryUser entity);
    }
}
