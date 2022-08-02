using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<ELibraryRole> Get(int id);
    }
}
