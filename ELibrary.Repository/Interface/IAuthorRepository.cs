using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author> GetWithBooks(int id);
    }
}
