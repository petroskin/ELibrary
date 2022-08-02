using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IUserRepository
    {
        Task<ELibraryUser> GetLatest();
    }
}
