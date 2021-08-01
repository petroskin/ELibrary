using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Interface
{
    public interface ICartService
    {
        void AddToCart(string userId, Guid bookId);
        void RemoveFromCart(string userId, Guid Id);
        void ClearCart(string userId);
        Cart getCart(string userId);
    }
}
