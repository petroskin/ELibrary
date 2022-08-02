using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface ICartService
    {
        Task AddToCart(string userId, int bookId);
        Task RemoveFromCart(int id);
        Task<Cart> getCart(string userId);
    }
}
