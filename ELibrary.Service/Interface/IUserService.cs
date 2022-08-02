using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IUserService
    {
        Task<ELibraryUser> Get(string id);
        IEnumerable<ELibraryUser> GetUsers();
        IEnumerable<ELibraryRole> GetRoles();
        Task ChangeRole(ELibraryUser user, string currentRole, string futureRole);
        Task<ELibraryUserDto> GetDto(string id);
        Task InsertFromDtoAsync(IEnumerable<ExcelUserDataDto> entities);
        //Task UpgradeStatus(string userId);
    }
}
