using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IBookCartRepository
    {
        Task Add(int cartId, int bookId);
        Task Remove(int id);
        Task RemoveAllFromCart(int cartId);
        Task<bool> IsPresent(int cartId, int bookId);
        Task<IEnumerable<BooksInCart>> GetByCart(int cartId);
    }
}
