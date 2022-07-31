using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetByPublisherId(int id);
    }
}
