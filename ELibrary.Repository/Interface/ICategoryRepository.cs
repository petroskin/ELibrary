using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetWithBooks(int id);
    }
}
