using ELibrary.Domain.DTO;
using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IRentService
    {
        Task<IEnumerable<Rent>> GetAll(string userId);
        Task<IEnumerable<Rent>> GetCurrentRentsByUser(string userId);
        Task RentNow(ELibraryUserDto dto);
    }
}
