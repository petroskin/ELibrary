using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Cart> Get(int id);
        Task Create(int id);
    }
}
