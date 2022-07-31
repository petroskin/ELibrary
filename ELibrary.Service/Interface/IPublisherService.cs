using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAll();
        Task<Publisher> Get(int id);
        Task<Publisher> GetWithBooks(int id);
        Task Insert(Publisher entity);
        Task Update(Publisher entity);
        Task Delete(Publisher entity);
        Task Delete(int id);
    }
}
