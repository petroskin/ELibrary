using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAll();
        Task<IEnumerable<Review>> GetAllByBook(int id);
        Task<IEnumerable<Review>> GetAllByUser(int id);
        Task<Review> Get(int id);
        Task Insert(Review entity);
        Task Update(Review entity);
        Task Delete(Review entity);
        Task Delete(int id);
    }
}
